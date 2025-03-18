using System;
using System.Windows;
using Project.Models;

namespace Project.Views
{
    public partial class TeacherAddCourseWindow : Window
    {
        public int TeacherId { get; set; }
        public TeacherAddCourseWindow(int teacherId)
        {
            InitializeComponent();
            TeacherId = teacherId;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string courseName = txtCourseName.Text.Trim();
            DateTime? startDate = dpStartDate.SelectedDate;
            DateTime? endDate = dpEndDate.SelectedDate;

            if (string.IsNullOrEmpty(courseName) || !startDate.HasValue || !endDate.HasValue)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Thông báo");
                return;
            }
            if (startDate.Value >= endDate.Value)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn ngày kết thúc.", "Thông báo");
                return;
            }
            try
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    Course newCourse = new Course
                    {
                        CourseName = courseName,
                        TeacherId = TeacherId,
                        StartDate = DateOnly.FromDateTime(startDate.Value),
                        EndDate = DateOnly.FromDateTime(endDate.Value)
                    };

                    context.Courses.Add(newCourse);
                    context.SaveChanges();
                }
                MessageBox.Show("Thêm khóa học thành công.", "Thông báo");
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm khóa học: " + ex.Message, "Lỗi");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
