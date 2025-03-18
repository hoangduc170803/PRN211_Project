using Project.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class TeacherStudentViewModel : BaseViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Class { get; set; }
        public string School { get; set; }
        public string CoursesEnrolled { get; set; }
    }
}
