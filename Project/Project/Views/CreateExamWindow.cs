using System.Windows;
using Project.ViewModels;

namespace Project.Views
{
    public partial class CreateExamWindow : Window
    {
        public CreateExamWindow(int teacherId)
        {
            InitializeComponent();
            var viewModel = new CreateExamViewModel(teacherId);
            viewModel.ExamSaved += () => this.Close();
            DataContext = viewModel;
        }
    }
}