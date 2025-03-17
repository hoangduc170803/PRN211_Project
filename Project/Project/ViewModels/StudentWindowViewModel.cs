using Project.Helpers;

namespace Project.ViewModels
{
    public class StudentWindowViewModel : BaseViewModel
    {
        // Thuộc tính Profile chứa thông tin cá nhân của học sinh
        public StudentProfileViewModel Profile { get; set; }

        public StudentWindowViewModel(int userId)
        {
            // Khởi tạo Profile dựa trên userId (được lấy sau khi đăng nhập)
            Profile = new StudentProfileViewModel(userId);
        }
    }
}