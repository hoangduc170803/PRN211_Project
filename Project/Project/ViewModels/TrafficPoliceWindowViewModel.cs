using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Project.Helpers;
using Project.Models;

namespace Project.ViewModels
{
    public class TrafficPoliceWindowViewModel : BaseViewModel
    {
        public ObservableCollection<Exam> Exams { get; set; }
        public ObservableCollection<Certificate> Certificates { get; set; }

        public ICommand ConfirmExamCommand { get; }
        public ICommand ApproveCertificateCommand { get; }

        public TrafficPoliceWindowViewModel()
        {
            Exams = new ObservableCollection<Exam>();
            Certificates = new ObservableCollection<Certificate>();

            ConfirmExamCommand = new RelayCommand(o => ConfirmExam(o));
            ApproveCertificateCommand = new RelayCommand(o => ApproveCertificate(o));

            LoadExams();
            LoadCertificates();
        }

        private void LoadExams()
        {
            try
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    // Giả sử kỳ thi cần được xác nhận là những kỳ thi có IsConfirmed = false
                    var exams = context.Exams
                        .Include(e => e.Course)
                        .Where(e => !e.IsConfirmed)
                        .ToList();
                    Exams.Clear();
                    foreach (var exam in exams)
                        Exams.Add(exam);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load kỳ thi: " + ex.Message);
            }
        }

        private void LoadCertificates()
        {
            try
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    // Giả sử các chứng chỉ cần được phê duyệt là những chứng chỉ có IsApproved = false
                    var certificates = context.Certificates
                        .Where(c => !c.IsApproved)
                        .ToList();
                    Certificates.Clear();
                    foreach (var cert in certificates)
                        Certificates.Add(cert);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load chứng chỉ: " + ex.Message);
            }
        }

        private void ConfirmExam(object examObj)
        {
            if (examObj is Exam exam)
            {
                try
                {
                    using (var context = new SafeDriveCertDbContext())
                    {
                        var examToUpdate = context.Exams.FirstOrDefault(e => e.ExamId == exam.ExamId);
                        if (examToUpdate != null)
                        {
                            // Xác nhận kỳ thi
                            examToUpdate.IsConfirmed = true;
                            context.SaveChanges();
                        }
                    }
                    LoadExams();
                    MessageBox.Show("Kỳ thi đã được xác nhận.", "Thông báo");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xác nhận kỳ thi: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn kỳ thi cần xác nhận.", "Thông báo");
            }
        }

        private void ApproveCertificate(object certificateObj)
        {
            if (certificateObj is Certificate cert)
            {
                try
                {
                    using (var context = new SafeDriveCertDbContext())
                    {
                        var certToUpdate = context.Certificates.FirstOrDefault(c => c.CertificateId == cert.CertificateId);
                        if (certToUpdate != null)
                        {
                            // Phê duyệt chứng chỉ
                            certToUpdate.IsApproved = true;
                            context.SaveChanges();
                        }
                    }
                    LoadCertificates();
                    MessageBox.Show("Chứng chỉ đã được phê duyệt.", "Thông báo");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi phê duyệt chứng chỉ: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn chứng chỉ cần phê duyệt.", "Thông báo");
            }
        }
    }
}
