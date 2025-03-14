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
    public class CoursesViewModel : BaseViewModel
    {
        private ObservableCollection<Course> _courses;
        public ObservableCollection<Course> Courses
        {
            get => _courses;
            set { _courses = value; OnPropertyChanged(nameof(Courses)); }
        }

        // Thuộc tính dùng để binding dữ liệu nhập từ giao diện cho đối tượng Course mới
        private Course _newCourse;
        public Course NewCourse
        {
            get => _newCourse;
            set { _newCourse = value; OnPropertyChanged(nameof(NewCourse)); }
        }

        public ICommand LoadCoursesCommand { get; set; }
        public ICommand SaveNewCourseCommand { get; set; }
        public ICommand DeleteCourseCommand { get; set; }

        public ICommand CreateNewCourseCommand { get; set; }

        public CoursesViewModel()
        {
            Courses = new ObservableCollection<Course>();
            NewCourse = new Course(); // Khởi tạo đối tượng rỗng để binding
            LoadCoursesCommand = new RelayCommand(o => LoadCourses());
            SaveNewCourseCommand = new RelayCommand(o => SaveNewCourse(), o => CanSaveNewCourse());
            DeleteCourseCommand = new RelayCommand(o => DeleteCourse(o));

            // Command reset đối tượng NewCourse, ví dụ khi nhấn "New Course"
            CreateNewCourseCommand = new RelayCommand(o => NewCourse = new Course());
        }

        public void LoadCourses()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                var list = context.Courses.ToList();
                Courses.Clear();
                foreach (var course in list)
                    Courses.Add(course);
            }
        }

        // Kiểm tra điều kiện lưu (ví dụ: các trường bắt buộc không được để trống)
        public bool CanSaveNewCourse()
        {
            // Ví dụ: CourseName không được rỗng, TeacherID phải > 0 và StartDate < EndDate
            return !string.IsNullOrWhiteSpace(NewCourse?.CourseName)
                   && NewCourse.TeacherId > 0
                   && NewCourse.StartDate < NewCourse.EndDate;
        }

        // Lưu đối tượng NewCourse được binding từ giao diện vào CSDL
        public void SaveNewCourse()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                context.Courses.Add(NewCourse);
                context.SaveChanges();
                Courses.Add(NewCourse);
            }
            // Reset đối tượng sau khi lưu để sẵn sàng nhập dữ liệu mới
            NewCourse = new Course();
        }

        public void DeleteCourse(object parameter)
        {
            if (parameter is Course course)
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    context.Courses.Remove(course);
                    context.SaveChanges();
                }
                Courses.Remove(course);
            }
        }
    }
}
