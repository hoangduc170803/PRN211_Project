using System.Linq;
using System.Windows;
using System.Windows.Input;
using Project.Helpers;   // Chứa BaseViewModel và RelayCommand
using Project.Models;    // Chứa model User, giả sử Role được lưu dưới dạng chuỗi ("Student", "Teacher", "TrafficPolice", "Admin")
using Project.Views;     // Chứa các Window: StudentWindow, TeacherWindow, TrafficPoliceWindow, AdminWindow

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
            // Khởi tạo command với phương thức thực thi
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private void ExecuteLogin(object parameter)
        {
            // Kiểm tra nếu Email hoặc Password trống
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Email hoặc Password không được để trống.",
                                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new SafeDriveCertDbContext())
            {
                // Tìm kiếm user theo Email và Password (trong ứng dụng thực tế, Password nên được mã hóa)
                var user = context.Users.FirstOrDefault(u => u.Email == Email && u.Password == Password);
                if (user != null)
                {
                    // Dựa vào vai trò, mở cửa sổ tương ứng
                    switch (user.Role.ToLower())
                    {
                        case "student":
                            new StudentWindow(user.UserId).Show();
                            break;
                        case "teacher":
                            new TeacherWindow(user.UserId).Show();
                            break;
                        case "trafficpolice":
                            new TrafficPoliceWindow().Show();
                            break;
                        case "admin":
                            new AdminWindow().Show();
                            break;
                        default:
                            MessageBox.Show("Vai trò người dùng không hợp lệ.",
                                            "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                    }

                    // Đóng cửa sổ Login hiện tại
                    var loginWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is LoginWindow);
                    loginWindow?.Close();
                }
                else
                {
                    MessageBox.Show("Email hoặc Password không đúng.",
                                    "Đăng nhập thất bại", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
