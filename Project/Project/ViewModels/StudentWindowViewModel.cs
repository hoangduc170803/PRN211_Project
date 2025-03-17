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
        // Thuộc tính chứa danh sách khóa học hiển thị (bao gồm thông tin bổ sung)
        public ObservableCollection<CourseDisplayViewModel> CoursesDisplay { get; set; }
        // Các thuộc tính khác (ví dụ: Exams, Registrations) nếu cần...

        public ICommand LoadCoursesDisplayCommand { get; }
        public ICommand RegisterCourseCommand { get; }

        public StudentWindowViewModel(int userId)
        {
            Profile = new StudentProfileViewModel(userId);
            CoursesDisplay = new ObservableCollection<CourseDisplayViewModel>();

            LoadCoursesDisplayCommand = new RelayCommand(o => LoadCoursesDisplay());
            RegisterCourseCommand = new RelayCommand(o => RegisterCourse((int)o));

            LoadCoursesDisplay();
        }

        private void LoadCoursesDisplay()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                // Load các khóa học với thông tin giảng viên và kỳ thi
                var courses = context.Courses.Include(c => c.Teacher).Include(c => c.Exams).ToList();
                // Lấy thông tin đăng ký của học sinh hiện tại
                var regs = context.Registrations.Where(r => r.UserId == Profile.UserId).ToList();

                CoursesDisplay.Clear();
                foreach (var course in courses)
                {
                    // Giả sử ExamTime là ngày thi của kỳ thi đầu tiên, nếu có
                    var examTime = course.Exams.FirstOrDefault()?.Date ?? default;
                    // Tìm đăng ký của học sinh với khóa học này
                    var reg = regs.FirstOrDefault(r => r.CourseId == course.CourseId);
                    string regStatus = reg != null ? reg.Status : "Chưa đăng ký";

                    CoursesDisplay.Add(new CourseDisplayViewModel
                    {
                        CourseId = course.CourseId,
                        CourseName = course.CourseName,
                        TeacherName = course.Teacher?.FullName,
                        StartDate = course.StartDate.ToDateTime(new TimeOnly(0, 0)),
                        EndDate = course.EndDate.ToDateTime(new TimeOnly(0, 0)),
                        ExamTime = examTime.ToDateTime(new TimeOnly(0, 0)),
                        RegistrationStatus = regStatus
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
        }
    }
}
