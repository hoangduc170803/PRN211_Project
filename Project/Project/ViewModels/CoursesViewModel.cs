using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Project.Helpers;    // Chứa BaseViewModel, RelayCommand
using Project.Models;     // Chứa model Course

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

        // Thuộc tính binding cho dữ liệu nhập của khóa học mới
        private Course _newCourse;
        public Course NewCourse
        {
            get => _newCourse;
            set { _newCourse = value; OnPropertyChanged(nameof(NewCourse)); }
        }

        // Các lệnh (Commands)
        public ICommand LoadCoursesCommand { get; set; }
        public ICommand SaveNewCourseCommand { get; set; }
        public ICommand DeleteCourseCommand { get; set; }
        public ICommand CreateNewCourseCommand { get; set; }

        public CoursesViewModel()
        {
            // Khởi tạo danh sách khóa học và đối tượng NewCourse
            Courses = new ObservableCollection<Course>();
            NewCourse = new Course();

            // Khởi tạo các command
            LoadCoursesCommand = new RelayCommand(o => LoadCourses());
            SaveNewCourseCommand = new RelayCommand(o => SaveNewCourse(), o => CanSaveNewCourse());
            DeleteCourseCommand = new RelayCommand(o => DeleteCourse(o));
            CreateNewCourseCommand = new RelayCommand(o => NewCourse = new Course());

            // Load dữ liệu ban đầu
            LoadCourses();
        }

        /// <summary>
        /// Load danh sách khóa học từ database.
        /// </summary>
        public void LoadCourses()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                var list = context.Courses.ToList();
                Courses.Clear();
                foreach (var course in list)
                {
                    Courses.Add(course);
                }
            }
        }

        /// <summary>
        /// Kiểm tra điều kiện để lưu khóa học mới.
        /// Ví dụ: tên khóa học không trống, TeacherId > 0 và StartDate < EndDate.
        /// </summary>
        public bool CanSaveNewCourse()
        {
            return !string.IsNullOrWhiteSpace(NewCourse?.CourseName)
                   && NewCourse.TeacherId > 0
                   && NewCourse.StartDate < NewCourse.EndDate;
        }

        /// <summary>
        /// Lưu khóa học mới vào database.
        /// Sau khi lưu, thêm đối tượng vào ObservableCollection và reset NewCourse.
        /// </summary>
        public void SaveNewCourse()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                context.Courses.Add(NewCourse);
                context.SaveChanges();
                // Thêm đối tượng mới vào danh sách hiển thị
                Courses.Add(NewCourse);
            }
            // Reset NewCourse để sẵn sàng cho lần nhập mới
            NewCourse = new Course();
        }

        /// <summary>
        /// Xóa khóa học khỏi database và danh sách hiển thị.
        /// Parameter cần là một đối tượng Course.
        /// </summary>
        /// <param name="parameter"></param>
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
