using Project.Helpers;

namespace Project.ViewModels
{
    public class LearningResultViewModel : BaseViewModel
    {
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public decimal Score { get; set; }
        public bool PassStatus { get; set; }
        public string CertificateCode { get; set; }
        // Thuộc tính mới: chứng chỉ đã được phê duyệt hay chưa
        public bool CertificateApproved { get; set; }
    }
}
