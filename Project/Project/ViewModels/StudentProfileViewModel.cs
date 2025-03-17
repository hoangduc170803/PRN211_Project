using System.Linq;
using System.Windows;
using System.Windows.Input;
using Project.Helpers;
using Project.Models;

namespace Project.ViewModels
{
    public class StudentProfileViewModel : BaseViewModel
    {
        private int _userId;
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Class { get; set; }
        public string School { get; set; }

        public ICommand UpdateProfileCommand { get; }

        public int UserId
        {
            get { return _userId; }
        }

        public StudentProfileViewModel(int userId)
        {
            _userId = userId;
            LoadProfile();
            UpdateProfileCommand = new RelayCommand(UpdateProfile);
        }

        private void LoadProfile()
        {
            try
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    var user = context.Users.FirstOrDefault(u => u.UserId == _userId);
                    if (user != null)
                    {
                        FullName = user.FullName;
                        Email = user.Email;
                        Phone = user.Phone;
                        Class = user.Class;
                        School = user.School;
                        OnPropertyChanged(nameof(FullName));
                        OnPropertyChanged(nameof(Email));
                        OnPropertyChanged(nameof(Phone));
                        OnPropertyChanged(nameof(Class));
                        OnPropertyChanged(nameof(School));
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi khi load thông tin: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateProfile(object parameter)
        {
            try
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    var user = context.Users.FirstOrDefault(u => u.UserId == _userId);
                    if (user != null)
                    {
                        user.FullName = FullName;
                        user.Phone = Phone;
                        user.Class = Class;
                        user.School = School;
                        context.SaveChanges();
                        MessageBox.Show("Thông tin cá nhân đã được cập nhật.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật thông tin: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
