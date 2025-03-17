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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        // Phương thức xử lý sự kiện PasswordChanged
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            // Ví dụ: cập nhật Password vào ViewModel
            if (DataContext is LoginViewModel vm && sender is PasswordBox pwd)
            {
                vm.Password = pwd.Password;
            }
        }
    }
}
