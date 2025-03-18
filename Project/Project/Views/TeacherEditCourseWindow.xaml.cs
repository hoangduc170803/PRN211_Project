using System;
using System.Windows;
using Project.Models;

namespace Project.Views
{
    public partial class TeacherEditCourseWindow : Window
    {
        public Course Course { get; set; }
        public TeacherEditCourseWindow(Course course)
        {
            InitializeComponent();
            // Tạo bản sao của course để tránh thay đổi trực tiếp trong danh sách trước khi lưu
            Course = new Course
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                TeacherId = course.TeacherId,
                StartDate = course.StartDate,
                EndDate = course.EndDate
            };
            DataContext = Course;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Lấy thông tin từ TextBox và DatePicker đã binding
            if (string.IsNullOrWhiteSpace(Course.CourseName))
            {
                MessageBox.Show("Tên khóa học không được để trống.", "Thông báo");
                return;
            }
            if (!dpStartDate.SelectedDate.HasValue || !dpEndDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Vui lòng chọn ngày bắt đầu và ngày kết thúc.", "Thông báo");
                return;
            }
            if (dpStartDate.SelectedDate.Value >= dpEndDate.SelectedDate.Value)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn ngày kết thúc.", "Thông báo");
                return;
            }

            // Nếu dùng converter, Course.StartDate và EndDate sẽ được cập nhật từ SelectedDate
            // Đóng cửa sổ với DialogResult = true để báo lưu thành công
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
