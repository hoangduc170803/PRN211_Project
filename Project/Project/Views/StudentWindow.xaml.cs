using Project.ViewModels;
using System.Windows;

namespace Project.Views
{
    /// <summary>
    /// Interaction logic for StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        public StudentWindow(int userId)
        {
            InitializeComponent();
            // Gán DataContext cho toàn bộ cửa sổ với StudentWindowViewModel có thuộc tính Profile
            DataContext = new StudentWindowViewModel(userId);
        }
    }
}
