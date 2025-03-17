
using Project.Helpers;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get => _users;
            set { _users = value; OnPropertyChanged(nameof(Users)); }
        }

        // Thuộc tính dùng để binding dữ liệu nhập từ người dùng cho đối tượng mới
        private User _newUser;
        public User NewUser
        {
            get => _newUser;
            set { _newUser = value; OnPropertyChanged(nameof(NewUser)); }
        }

        public ICommand LoadUsersCommand { get; set; }
        public ICommand SaveNewUserCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }

        public ICommand CreateNewUserCommand { get; set; }

        public UsersViewModel()
        {
            Users = new ObservableCollection<User>();
            NewUser = new User(); // Khởi tạo đối tượng rỗng cho binding
            LoadUsersCommand = new RelayCommand(o => LoadUsers());
            SaveNewUserCommand = new RelayCommand(o => SaveNewUser(), o => CanSaveNewUser());
            DeleteUserCommand = new RelayCommand(o => DeleteUser(o));

            // Command reset đối tượng NewUser, ví dụ khi nhấn "New User"
            CreateNewUserCommand = new RelayCommand(o => NewUser = new User());
        }

        public void LoadUsers()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                var list = context.Users.ToList();
                Users.Clear();
                foreach (var user in list)
                    Users.Add(user);
            }
        }

        // Kiểm tra điều kiện lưu (ví dụ: các trường bắt buộc không được để trống)
        public bool CanSaveNewUser()
        {
            return !string.IsNullOrWhiteSpace(NewUser?.FullName) &&
                   !string.IsNullOrWhiteSpace(NewUser?.Email) &&
                   !string.IsNullOrWhiteSpace(NewUser?.Password);
        }

        // Lưu đối tượng NewUser đã được binding từ giao diện vào CSDL
        public void SaveNewUser()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                context.Users.Add(NewUser);
                context.SaveChanges();
                Users.Add(NewUser);
            }
            // Sau khi lưu, reset đối tượng để sẵn sàng nhập dữ liệu mới
            NewUser = new User();
        }

        public void DeleteUser(object parameter)
        {
            if (parameter is User user)
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    context.Users.Remove(user);
                    context.SaveChanges();
                }
                Users.Remove(user);
            }
        }
    }
}
