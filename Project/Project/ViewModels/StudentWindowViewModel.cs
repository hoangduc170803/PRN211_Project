using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Project.Helpers;
using Project.Models;

namespace Project.ViewModels
{
    public class StudentWindowViewModel : BaseViewModel
    {
        public StudentProfileViewModel Profile { get; set; }
        public ObservableCollection<CourseDisplayViewModel> CoursesDisplay { get; set; }
        public ObservableCollection<LearningProgressItemViewModel> LearningProgress { get; set; }
        public ICommand LoadCoursesDisplayCommand { get; }
        public ICommand RegisterCourseCommand { get; }

        public StudentWindowViewModel(int userId)
        {
            Profile = new StudentProfileViewModel(userId);
            CoursesDisplay = new ObservableCollection<CourseDisplayViewModel>();
            LearningProgress = new ObservableCollection<LearningProgressItemViewModel>();

            LoadCoursesDisplayCommand = new RelayCommand(o => LoadCoursesDisplay());
            RegisterCourseCommand = new RelayCommand(o => RegisterCourse((int)o));

            LoadCoursesDisplay();
            LoadLearningProgress();
        }

        private void LoadCoursesDisplay()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                // Load các khóa học với thông tin giảng viên và kỳ thi
                var courses = context.Courses.Include(c => c.Teacher)
                                             .Include(c => c.Exams)
                                             .ToList();
                var regs = context.Registrations.Where(r => r.UserId == Profile.UserId).ToList();

                CoursesDisplay.Clear();
                foreach (var course in courses)
                {
                    var examTime = course.Exams.FirstOrDefault()?.Date.ToDateTime(new TimeOnly(0, 0)) ?? default;
                    var reg = regs.FirstOrDefault(r => r.CourseId == course.CourseId);
                    string regStatus = reg != null ? reg.Status : "Chưa đăng ký";

                    CoursesDisplay.Add(new CourseDisplayViewModel
                    {
                        CourseId = course.CourseId,
                        CourseName = course.CourseName,
                        TeacherName = course.Teacher?.FullName,
                        StartDate = course.StartDate.ToDateTime(new TimeOnly(0, 0)),
                        EndDate = course.EndDate.ToDateTime(new TimeOnly(0, 0)),
                        ExamTime = examTime,
                        RegistrationStatus = regStatus
                    });
                }
            }
        }

        private void LoadLearningProgress()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                // Lấy các đăng ký của học sinh cùng Course, Teacher, Exams
                var regs = context.Registrations
                                  .Where(r => r.UserId == Profile.UserId)
                                  .Include(r => r.Course)
                                    .ThenInclude(c => c.Teacher)
                                  .Include(r => r.Course)
                                    .ThenInclude(c => c.Exams)
                                  .ToList();

                LearningProgress.Clear();
                foreach (var reg in regs)
                {
                    var firstExam = reg.Course.Exams.FirstOrDefault();
                    var examDate = firstExam != null
                        ? firstExam.Date.ToDateTime(new TimeOnly(0, 0))
                        : default(DateTime);

                    // Lấy kết quả thi của học sinh nếu có
                    int? examId = firstExam != null ? firstExam.ExamId : (int?)null;
                    var result = examId.HasValue
                        ? context.Results.FirstOrDefault(r => r.UserId == Profile.UserId && r.ExamId == examId.Value)
                        : null;

                    // Chỉ lấy chứng chỉ nếu trạng thái đăng ký là "approved" và có kỳ thi
                    string certificateCode = null;
                    if (reg.Status.ToLower() == "approved" && firstExam != null)
                    {
                        // Nếu bảng Certificates có liên kết với khóa học hoặc kỳ thi, bạn cần thêm điều kiện liên kết ở đây.
                        // Ví dụ, nếu chứng chỉ được cấp theo CourseId:
                        var certificate = context.Certificates
                                                 .FirstOrDefault(c => c.UserId == Profile.UserId && c.ExamId == firstExam.ExamId);
                        certificateCode = certificate?.CertificateCode;
                    }

                    LearningProgress.Add(new LearningProgressItemViewModel
                    {
                        CourseId = reg.Course.CourseId,
                        CourseName = reg.Course.CourseName,
                        TeacherName = reg.Course.Teacher?.FullName,
                        ExamDate = examDate,
                        Score = result?.Score,
                        PassStatus = result?.PassStatus,
                        CertificateCode = certificateCode, // Chỉ có giá trị nếu đăng ký đã được phê duyệt
                        RegistrationStatus = reg.Status
                    });
                }
            }
        }




        private void RegisterCourse(int courseId)
        {
            using (var context = new SafeDriveCertDbContext())
            {
                var exists = context.Registrations.FirstOrDefault(r => r.UserId == Profile.UserId && r.CourseId == courseId);
                if (exists == null)
                {
                    var reg = new Registration { UserId = Profile.UserId, CourseId = courseId, Status = "Pending" };
                    context.Registrations.Add(reg);
                    context.SaveChanges();
                }
            }
            LoadCoursesDisplay();
            LoadLearningProgress();
        }
    }
}
