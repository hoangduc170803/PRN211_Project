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
    public class TeacherManagementViewModel : BaseViewModel
    {
        public int TeacherId { get; set; }
        public ObservableCollection<Course> Courses { get; set; }
        public ObservableCollection<TeacherStudentViewModel> Students { get; set; }

        public ICommand AddCourseCommand { get; }
        public ICommand EditCourseCommand { get; }

        public TeacherManagementViewModel(int teacherId)
        {
            TeacherId = teacherId;
            Courses = new ObservableCollection<Course>();
            Students = new ObservableCollection<TeacherStudentViewModel>();

            AddCourseCommand = new RelayCommand(o => AddCourse());
            EditCourseCommand = new RelayCommand(o => EditCourse(o));

            LoadCourses();
            LoadStudents();
        }

        private void LoadCourses()
        {
            try
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    var courses = context.Courses
                        .Where(c => c.TeacherId == TeacherId)
                        .ToList();
                    Courses.Clear();
                    foreach (var course in courses)
                    {
                        Courses.Add(course);
                    }
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
                    // Lấy danh sách đăng ký của các khóa học của giảng viên
                    var registrations = context.Registrations
                        .Include(r => r.User)
                        .Include(r => r.Course)
                        .Where(r => r.Course.TeacherId == TeacherId)
                        .ToList();

                    // Nhóm theo User
                    var studentGroups = registrations.GroupBy(r => r.User);

                    Students.Clear();
                    foreach (var group in studentGroups)
                    {
                        var student = group.Key;
                        // Lấy danh sách tên khóa học (distinct)
                        var coursesEnrolled = string.Join(", ", group.Select(r => r.Course.CourseName).Distinct());

                        Students.Add(new TeacherStudentViewModel
                        {
                            FullName = student.FullName,
                            Email = student.Email,
                            Class = student.Class,
                            School = student.School,
                            CoursesEnrolled = coursesEnrolled
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh sách học sinh: " + ex.Message);
            }
        }

        private void AddCourse()
        {
            var addWindow = new Views.TeacherAddCourseWindow(TeacherId);
            if (addWindow.ShowDialog() == true)
            {
                LoadCourses();
            }
        }

        private void EditCourse(object courseObj)
        {
            if (courseObj is Course course)
            {
                var editWindow = new Views.TeacherEditCourseWindow(course);
                if (editWindow.ShowDialog() == true)
                {
                    try
                    {
                        using (var context = new SafeDriveCertDbContext())
                        {
                            var courseToUpdate = context.Courses.FirstOrDefault(c => c.CourseId == course.CourseId);
                            if (courseToUpdate != null)
                            {
                                courseToUpdate.CourseName = editWindow.Course.CourseName;
                                courseToUpdate.StartDate = editWindow.Course.StartDate;
                                courseToUpdate.EndDate = editWindow.Course.EndDate;
                                context.SaveChanges();
                            }
                        }
                        LoadCourses();
                        LoadStudents();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi lưu khóa học: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khóa học để sửa.", "Thông báo");
            }
        }

    }
}
