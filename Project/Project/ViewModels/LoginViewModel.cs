using Project.Helpers;
using Project.Models;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Project.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email;
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private void ExecuteLogin(object parameter)
        {
            // Kiểm tra rỗng
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Email hoặc Password không được để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new SafeDriveCertDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Email == Email && u.Password == Password);
                if (user != null)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    Application.Current.Windows[0]?.Close();
                }
                else
                {
                    MessageBox.Show("Email hoặc Password không đúng", "Đăng nhập thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
