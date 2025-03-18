using System.Windows;
using Project.ViewModels;

namespace Project.Views
{
    public partial class TeacherManagementWindow : Window
    {
        public TeacherManagementWindow(int teacherId)
        {
            InitializeComponent();
            DataContext = new TeacherManagementViewModel(teacherId);
        }
    }
}
