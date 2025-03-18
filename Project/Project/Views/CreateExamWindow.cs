using System.Windows;
using Project.ViewModels;

namespace Project.Views
{
    public partial class CreateExamWindow : Window
    {
        public CreateExamWindow(int teacherId)
        {
            InitializeComponent();
            DataContext = new CreateExamViewModel(teacherId);
        }
    }

}
