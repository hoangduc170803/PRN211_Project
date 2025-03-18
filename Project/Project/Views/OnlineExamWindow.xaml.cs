using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Project.ViewModels;

namespace Project.Views
{
    public partial class OnlineExamWindow : Window
    {
        public OnlineExamWindow(int examId, int userId)
        {
            InitializeComponent();
            DataContext = new OnlineExamViewModel(examId, userId);
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton rb && rb.Tag != null && int.TryParse(rb.Tag.ToString(), out int optionId))
            {
                // Thay vì tìm ContentPresenter, hãy tìm Border (hoặc một container khác) mà DataContext là OnlineExamQuestionViewModel
                var parent = FindParent<Border>(rb);
                if (parent != null && parent.DataContext is OnlineExamQuestionViewModel qvm)
                {
                    qvm.SelectedOptionId = optionId;
                    System.Diagnostics.Debug.WriteLine($"QuestionId: {qvm.Question.QuestionId} updated SelectedOptionId = {qvm.SelectedOptionId}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Không tìm thấy parent có DataContext là OnlineExamQuestionViewModel.");
                }
            }
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null)
                return null;
            if (parentObject is T parent)
                return parent;
            return FindParent<T>(parentObject);
        }

    }
}
