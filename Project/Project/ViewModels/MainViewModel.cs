using Project.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        // Khai báo các ViewModel con cho từng chức năng
        public UsersViewModel UsersVM { get; set; }
        public CoursesViewModel CoursesVM { get; set; }
        public RegistrationsViewModel RegistrationsVM { get; set; }
        public ExamsViewModel ExamsVM { get; set; }
        public ResultsViewModel ResultsVM { get; set; }
        public CertificatesViewModel CertificatesVM { get; set; }
        public NotificationsViewModel NotificationsVM { get; set; }

        public MainViewModel()
        {
            UsersVM = new UsersViewModel();
            CoursesVM = new CoursesViewModel();
            RegistrationsVM = new RegistrationsViewModel();
            ExamsVM = new ExamsViewModel();
            ResultsVM = new ResultsViewModel();
            CertificatesVM = new CertificatesViewModel();
            NotificationsVM = new NotificationsViewModel();
        }
    }
}
