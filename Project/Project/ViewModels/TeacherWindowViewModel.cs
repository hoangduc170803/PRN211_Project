using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Project.Helpers;
using Project.Models;

namespace Project.ViewModels
{
    public class TeacherWindowViewModel : BaseViewModel
    {
        public int TeacherId { get; set; }
        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<User> Students { get; set; }
        public ObservableCollection<LearningResultViewModel> LearningResults { get; set; }

        public ICommand AddCourseCommand { get; }
        public ICommand EditCourseCommand { get; }
        public ICommand DeleteCourseCommand { get; }
        public ICommand CreateExamCommand { get; }

        public TeacherWindowViewModel(int teacherId)
        {
            TeacherId = teacherId;
            Courses = new ObservableCollection<Course>();
            Students = new ObservableCollection<User>();
            LearningResults = new ObservableCollection<LearningResultViewModel>();

            CreateExamCommand = new RelayCommand(o => CreateExam());

            LoadCourses();
            LoadStudents();
            LoadLearningResults();
        }

        private void LoadCourses()
        {
            try
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    var courses = context.Courses.Where(c => c.TeacherId == TeacherId).ToList();
                    Courses.Clear();
                    foreach (var course in courses)
                        Courses.Add(course);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load khóa học: " + ex.Message);
            }
        }

        private void LoadStudents()
        {
            try
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    var students = context.Registrations
                        .Include(r => r.User)
                        .Include(r => r.Course)
                        .Where(r => r.Course.TeacherId == TeacherId)
                        .Select(r => r.User)
                        .Distinct()
                        .ToList();
                    Students.Clear();
                    foreach (var student in students)
                        Students.Add(student);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh sách học sinh: " + ex.Message);
            }
        }

        private void LoadLearningResults()
        {
            try
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    var results = context.Results
                        .Include(r => r.User)
                        .Include(r => r.Exam)
                            .ThenInclude(e => e.Course)
                        .Where(r => r.Exam.Course.TeacherId == TeacherId)
                        .ToList();

                    LearningResults.Clear();
                    foreach (var r in results)
                    {
                        var certificate = context.Certificates.FirstOrDefault(c => c.UserId == r.UserId && c.ExamId == r.ExamId);
                        LearningResults.Add(new LearningResultViewModel
                        {
                            StudentName = r.User.FullName,
                            CourseName = r.Exam.Course.CourseName,
                            Score = r.Score,
                            PassStatus = r.PassStatus,
                            CertificateCode = certificate?.CertificateCode
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load kết quả học tập: " + ex.Message);
            }
        }

       

        private void CreateExam()
        {
            MessageBox.Show("Chức năng tạo đề thi mới chưa được triển khai.", "Thông báo");
        }
    }
}
