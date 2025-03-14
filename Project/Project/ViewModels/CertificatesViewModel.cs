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
    public class CertificatesViewModel : BaseViewModel
    {
        public ObservableCollection<Certificate> Certificates { get; set; }

        // Thuộc tính binding cho đối tượng Certificate mới, không khởi tạo mặc định trong constructor
        private Certificate _newCertificate;
        public Certificate NewCertificate
        {
            get => _newCertificate;
            set { _newCertificate = value; OnPropertyChanged(nameof(NewCertificate)); }
        }

        public ICommand LoadCertificatesCommand { get; set; }
        public ICommand CreateNewCertificateCommand { get; set; }
        public ICommand SaveNewCertificateCommand { get; set; }
        public ICommand DeleteCertificateCommand { get; set; }

        public CertificatesViewModel()
        {
            Certificates = new ObservableCollection<Certificate>();
            // Không khởi tạo NewCertificate ở đây, chờ người dùng bấm nút "New Certificate"
            LoadCertificatesCommand = new RelayCommand(o => LoadCertificates());
            CreateNewCertificateCommand = new RelayCommand(o => CreateNewCertificate());
            SaveNewCertificateCommand = new RelayCommand(o => SaveNewCertificate(), o => CanSaveNewCertificate());
            DeleteCertificateCommand = new RelayCommand(o => DeleteCertificate(o));
        }

        public void LoadCertificates()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                var list = context.Certificates.ToList();
                Certificates.Clear();
                foreach (var cert in list)
                    Certificates.Add(cert);
            }
        }

        // Command tạo đối tượng mới, để binding trên giao diện
        private void CreateNewCertificate()
        {
            NewCertificate = new Certificate();
        }

        // Kiểm tra điều kiện lưu (ví dụ: các trường bắt buộc không được để trống)
        public bool CanSaveNewCertificate()
        {
            return NewCertificate != null &&
                   NewCertificate.UserId > 0 &&
                   NewCertificate.IssuedDate != DateOnly.FromDateTime(default(DateTime)) &&
                   NewCertificate.ExpirationDate != DateOnly.FromDateTime(default(DateTime)) &&
                   !string.IsNullOrWhiteSpace(NewCertificate.CertificateCode);
        }

        public void SaveNewCertificate()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                context.Certificates.Add(NewCertificate);
                context.SaveChanges();
                Certificates.Add(NewCertificate);
            }
            // Reset đối tượng sau khi lưu để sẵn sàng nhập đối tượng mới
            NewCertificate = null;
        }

        public void DeleteCertificate(object parameter)
        {
            if (parameter is Certificate cert)
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    context.Certificates.Remove(cert);
                    context.SaveChanges();
                }
                Certificates.Remove(cert);
            }
        }
    }
}
