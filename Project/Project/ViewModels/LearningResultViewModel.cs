using Project.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class LearningResultViewModel : BaseViewModel
    {
        public string StudentName { get; set; }
        public string CourseName { get; set; }
        public decimal Score { get; set; }
        public bool PassStatus { get; set; }
        public string CertificateCode { get; set; }
    }
}
