using Project.Helpers;
using System;

namespace Project.ViewModels
{
    public class LearningProgressItemViewModel : BaseViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public DateTime ExamDate { get; set; }
        public decimal? Score { get; set; } // Có thể null nếu chưa thi
        public bool? PassStatus { get; set; } // true, false, hoặc null nếu chưa thi
        public string CertificateCode { get; set; } // Nếu đã nhận chứng chỉ
        public string RegistrationStatus { get; set; }
    }
}
