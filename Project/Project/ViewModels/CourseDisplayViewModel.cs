using Project.Helpers;
using System;

namespace Project.ViewModels
{
    public class CourseDisplayViewModel : BaseViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExamTime { get; set; } 
        public string RegistrationStatus { get; set; }
    }
}