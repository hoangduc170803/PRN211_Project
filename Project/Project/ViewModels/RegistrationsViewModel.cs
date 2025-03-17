
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
    public class RegistrationsViewModel : BaseViewModel
    {
        private ObservableCollection<Registration> _registrations;
        public ObservableCollection<Registration> Registrations
        {
            get => _registrations;
            set { _registrations = value; OnPropertyChanged(nameof(Registrations)); }
        }

        public ICommand CreateNewRegistrationCommand { get; set; }

        // Đối tượng Registration mới được binding từ UI
        private Registration _newRegistration;
        public Registration NewRegistration
        {
            get => _newRegistration;
            set { _newRegistration = value; OnPropertyChanged(nameof(NewRegistration)); }
        }

        public ICommand LoadRegistrationsCommand { get; set; }
        public ICommand SaveNewRegistrationCommand { get; set; }
        public ICommand DeleteRegistrationCommand { get; set; }

        public RegistrationsViewModel()
        {
            Registrations = new ObservableCollection<Registration>();
            NewRegistration = new Registration(); // Khởi tạo đối tượng rỗng
            LoadRegistrationsCommand = new RelayCommand(o => LoadRegistrations());
            SaveNewRegistrationCommand = new RelayCommand(o => SaveNewRegistration(), o => CanSaveNewRegistration());
            DeleteRegistrationCommand = new RelayCommand(o => DeleteRegistration(o));

            // Command để tạo Registration mới (ví dụ: reset form)
            CreateNewRegistrationCommand = new RelayCommand(o => NewRegistration = new Registration());
        }

        public void LoadRegistrations()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                var list = context.Registrations.ToList();
                Registrations.Clear();
                foreach (var reg in list)
                    Registrations.Add(reg);
            }
        }

        public bool CanSaveNewRegistration()
        {
            // Kiểm tra các trường bắt buộc, ví dụ: UserID và CourseID phải hợp lệ
            return NewRegistration?.UserId > 0 && NewRegistration?.CourseId > 0;
        }

        public void SaveNewRegistration()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                context.Registrations.Add(NewRegistration);
                context.SaveChanges();
                Registrations.Add(NewRegistration);
            }
            NewRegistration = new Registration();
        }

        public void DeleteRegistration(object parameter)
        {
            if (parameter is Registration reg)
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    context.Registrations.Remove(reg);
                    context.SaveChanges();
                }
                Registrations.Remove(reg);
            }
        }
    }
}
