using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Project.Helpers;
using Project.Models;
using Project.Views;

namespace Project.ViewModels
{
    public class StudentWindowViewModel : BaseViewModel
    {

        private Exam _selectedExam;
        public Exam SelectedExam
        {
            get => _selectedExam;
            set { _selectedExam = value; OnPropertyChanged(nameof(SelectedExam)); }
        }

        public StudentProfileViewModel Profile { get; set; }
        public ObservableCollection<CourseDisplayViewModel> CoursesDisplay { get; set; }
        public ObservableCollection<LearningProgressItemViewModel> LearningProgress { get; set; }
        // Thêm collection cho kỳ thi
        public ObservableCollection<Exam> Exams { get; set; }

        public ICommand LoadCoursesDisplayCommand { get; }
        public ICommand RegisterCourseCommand { get; }
        public ICommand StartExamCommand { get; }

        public StudentWindowViewModel(int userId)
        {
            Profile = new StudentProfileViewModel(userId);
            CoursesDisplay = new ObservableCollection<CourseDisplayViewModel>();
            LearningProgress = new ObservableCollection<LearningProgressItemViewModel>();
            Exams = new ObservableCollection<Exam>();

            LoadCoursesDisplayCommand = new RelayCommand(o => LoadCoursesDisplay());
            RegisterCourseCommand = new RelayCommand(o => RegisterCourse((int)o));
            StartExamCommand = new RelayCommand(o => StartExam());

            // Load dữ liệu ban đầu
            LoadCoursesDisplay();
            LoadExams();
            LoadLearningProgress();
        }

        private void LoadCoursesDisplay()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                // Load khóa học với thông tin giảng viên và kỳ thi
                var courses = context.Courses
                                     .Include(c => c.Teacher)
                                     .Include(c => c.Exams)
                                     .ToList();
                var regs = context.Registrations.Where(r => r.UserId == Profile.UserId).ToList();

                CoursesDisplay.Clear();
                foreach (var course in courses)
                {
                    var examTime = course.Exams.FirstOrDefault()?.Date.ToDateTime(new TimeOnly(0, 0)) ?? default(DateTime);
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

        private void LoadExams()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                // Lấy danh sách các kỳ thi của các khóa học mà sinh viên đã đăng ký (approved)
                var approvedExamIds = context.Registrations
                                               .Where(r => r.UserId == Profile.UserId && r.Status.ToLower() == "approved")
                                               .SelectMany(r => r.Course.Exams.Select(e => e.ExamId))
                                               .Distinct()
                                               .ToList();

                // Chỉ load những kỳ thi có examId nằm trong approvedExamIds và đã được confirmed (IsConfirmed == true)
                var exams = context.Exams
                                   .Where(e => approvedExamIds.Contains(e.ExamId) && e.IsConfirmed)
                                   .Include(e => e.Course)
                                   .ThenInclude(c => c.Teacher)
                                   .ToList();

                Exams.Clear();
                foreach (var exam in exams)
                {
                    Exams.Add(exam);
                }
            }
        }



        private void LoadLearningProgress()
        {
            using (var context = new SafeDriveCertDbContext())
            {
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

                    int? examId = firstExam != null ? firstExam.ExamId : (int?)null;
                    var result = examId.HasValue
                        ? context.Results.FirstOrDefault(r => r.UserId == Profile.UserId && r.ExamId == examId.Value)
                        : null;

                    string certificateCode = null;
                    bool certificateApproved = false;
                    if (reg.Status.ToLower() == "approved" && result != null && result.PassStatus && firstExam != null)
                    {
                        var certificate = context.Certificates
                                                 .FirstOrDefault(c => c.UserId == Profile.UserId && c.ExamId == firstExam.ExamId);
                        certificateCode = certificate?.CertificateCode;
                        certificateApproved = certificate != null && certificate.IsApproved;
                    }

                    LearningProgress.Add(new LearningProgressItemViewModel
                    {
                        CourseId = reg.Course.CourseId,
                        CourseName = reg.Course.CourseName,
                        TeacherName = reg.Course.Teacher?.FullName,
                        ExamDate = examDate,
                        Score = result?.Score,
                        PassStatus = result?.PassStatus,
                        CertificateCode = certificateCode,
                        CertificateApproved = certificateApproved,
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

        private void StartExam()
        {
            if (SelectedExam == null)
            {
                MessageBox.Show("Hãy chọn kỳ thi để bắt đầu.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            using (var context = new SafeDriveCertDbContext())
            {
                bool isRegistered = context.Registrations.Any(r => r.UserId == Profile.UserId && r.CourseId == SelectedExam.CourseId);
                if (!isRegistered)
                {
                    MessageBox.Show("Bạn chưa đăng ký khóa học này. Vui lòng đăng ký trước khi thi.",
                                    "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                bool alreadyHasResult = context.Results.Any(r => r.UserId == Profile.UserId && r.ExamId == SelectedExam.ExamId);
                if (alreadyHasResult)
                {
                    MessageBox.Show("Bạn đã thi bài thi này rồi!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }

            new OnlineExamWindow(SelectedExam.ExamId, Profile.UserId).Show();
        }



    }
}
