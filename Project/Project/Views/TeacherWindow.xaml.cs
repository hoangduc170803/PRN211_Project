using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Project.Views
{
    /// <summary>
    /// Interaction logic for TeacherWindow.xaml
    /// </summary>
    public partial class TeacherWindow : Window
    {
        private int _teacherId;

        public TeacherWindow(int teacherId)
        {
            InitializeComponent();
            _teacherId = teacherId;
            DataContext = new TeacherWindowViewModel(teacherId);
        }

        private void ManageCoursesAndStudents_Click(object sender, RoutedEventArgs e)
        {
            // Mở cửa sổ quản lý khóa học và học sinh
            TeacherManagementWindow window = new TeacherManagementWindow(_teacherId);
            window.Show();
        }

        private void CreateExam_Click(object sender, RoutedEventArgs e)
        {
            // Mở cửa sổ tạo đề thi cho giảng viên
            CreateExamWindow examWindow = new CreateExamWindow();
            examWindow.Show();
        }
    }
}
