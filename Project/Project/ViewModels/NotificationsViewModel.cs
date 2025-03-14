using Project.Data;
using Project.Helpers;
using Project.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Project.ViewModels
{
    public class NotificationsViewModel : BaseViewModel
    {
        public ObservableCollection<Notification> Notifications { get; set; }

        // Thuộc tính binding cho đối tượng Notification mới
        private Notification _newNotification;
        public Notification NewNotification
        {
            get => _newNotification;
            set { _newNotification = value; OnPropertyChanged(nameof(NewNotification)); }
        }

        public ICommand LoadNotificationsCommand { get; set; }
        public ICommand CreateNewNotificationCommand { get; set; }
        public ICommand SaveNewNotificationCommand { get; set; }
        public ICommand DeleteNotificationCommand { get; set; }

        public NotificationsViewModel()
        {
            Notifications = new ObservableCollection<Notification>();
            // Không khởi tạo NewNotification trong constructor
            LoadNotificationsCommand = new RelayCommand(o => LoadNotifications());
            CreateNewNotificationCommand = new RelayCommand(o => CreateNewNotification());
            SaveNewNotificationCommand = new RelayCommand(o => SaveNewNotification(), o => CanSaveNewNotification());
            DeleteNotificationCommand = new RelayCommand(o => DeleteNotification(o));
        }

        public void LoadNotifications()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                var list = context.Notifications.ToList();
                Notifications.Clear();
                foreach (var note in list)
                    Notifications.Add(note);
            }
        }

        // Command tạo đối tượng Notification mới để binding
        private void CreateNewNotification()
        {
            NewNotification = new Notification();
        }

        // Kiểm tra điều kiện lưu (ví dụ: UserID và Message không được rỗng)
        public bool CanSaveNewNotification()
        {
            return NewNotification != null &&
                   NewNotification.UserId > 0 &&
                   !string.IsNullOrWhiteSpace(NewNotification.Message);
        }

        public void SaveNewNotification()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                context.Notifications.Add(NewNotification);
                context.SaveChanges();
                Notifications.Add(NewNotification);
            }
            // Reset đối tượng sau khi lưu
            NewNotification = null;
        }

        public void DeleteNotification(object parameter)
        {
            if (parameter is Notification note)
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    context.Notifications.Remove(note);
                    context.SaveChanges();
                }
                Notifications.Remove(note);
            }
        }
    }
}
