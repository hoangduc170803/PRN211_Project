using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Project.Helpers;
using Project.Models;

namespace Project.ViewModels
{
    public class CreateExamViewModel : BaseViewModel
    {
        public event Action ExamSaved;

        // Giá trị TeacherId được truyền từ cửa sổ giảng viên
        public int TeacherId { get; set; }

        // Danh sách khóa học của giảng viên
        public ObservableCollection<Course> Courses { get; set; }
        private Course _selectedCourse;
        public Course SelectedCourse
        {
            get => _selectedCourse;
            set { _selectedCourse = value; OnPropertyChanged(nameof(SelectedCourse)); }
        }

        // Thông tin kỳ thi
        private DateTime _examDate;
        public DateTime ExamDate
        {
            get => _examDate;
            set { _examDate = value; OnPropertyChanged(nameof(ExamDate)); }
        }

        private string _room;
        public string Room
        {
            get => _room;
            set { _room = value; OnPropertyChanged(nameof(Room)); }
        }

        // Danh sách câu hỏi của kỳ thi
        public ObservableCollection<Question> Questions { get; set; }

        // Các lệnh (Command)
        public ICommand AddQuestionCommand { get; }
        public ICommand AddOptionCommand { get; }
        public ICommand SelectImageCommand { get; }
        public ICommand SaveExamCommand { get; }

        public CreateExamViewModel(int teacherId)
        {
            TeacherId = teacherId;
            ExamDate = DateTime.Now;
            Room = string.Empty;

            Courses = new ObservableCollection<Course>();
            LoadCourses();

            // Tạo danh sách câu hỏi và pre-populate 30 câu hỏi rỗng
            Questions = new ObservableCollection<Question>();
            for (int i = 1; i <= 30; i++)
            {
                Questions.Add(new Question
                {
                    Content = $"Câu hỏi {i}:",
                    ImagePath = string.Empty,
                    Options = new ObservableCollection<Option>()
                });
            }

            AddQuestionCommand = new RelayCommand(o => AddQuestion());
            AddOptionCommand = new RelayCommand(o => AddOption(o));
            SelectImageCommand = new RelayCommand(o => SelectImage(o));
            SaveExamCommand = new RelayCommand(o => SaveExam());
        }

        private void LoadCourses()
        {
            try
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    // Load danh sách khóa học của giảng viên dựa trên TeacherId
                    var courses = context.Courses.Where(c => c.TeacherId == TeacherId).ToList();
                    Courses.Clear();
                    foreach (var course in courses)
                    {
                        Courses.Add(course);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load khóa học: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddQuestion()
        {
            if (Questions.Count < 30)
            {
                Questions.Add(new Question
                {
                    Content = "Nhập nội dung câu hỏi...",
                    ImagePath = string.Empty,
                    Options = new ObservableCollection<Option>()
                });
            }
            else
            {
                MessageBox.Show("Mỗi kỳ thi chỉ cho phép tối đa 30 câu hỏi.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddOption(object parameter)
        {
            if (parameter is Question question)
            {
                question.Options.Add(new Option
                {
                    OptionText = "Nhập nội dung đáp án...",
                    IsCorrect = false
                });
            }
        }

        private void SelectImage(object parameter)
        {
            if (parameter is Question question)
            {
                var dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (dialog.ShowDialog() == true)
                {
                    question.ImagePath = dialog.FileName;
                    OnPropertyChanged(nameof(Questions));
                }
            }
        }

        private void SaveExam()
        {
            if (string.IsNullOrWhiteSpace(Room))
            {
                MessageBox.Show("Vui lòng nhập phòng thi.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedCourse == null)
            {
                MessageBox.Show("Vui lòng chọn khóa học.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            foreach (var question in Questions)
            {
                if (string.IsNullOrWhiteSpace(question.Content))
                {
                    MessageBox.Show("Vui lòng nhập nội dung cho tất cả các câu hỏi.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            try
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    // Tạo đối tượng Exam mới với IsConfirmed = false
                    Exam exam = new Exam
                    {
                        Date = DateOnly.FromDateTime(ExamDate),
                        Room = Room,
                        CourseId = SelectedCourse.CourseId,
                        IsConfirmed = false // Kỳ thi mới chưa được xác nhận bởi cảnh sát giao thông
                    };
                    context.Exams.Add(exam);
                    context.SaveChanges();

                    foreach (var question in Questions)
                    {
                        question.ExamId = exam.ExamId;
                        context.Questions.Add(question);
                    }
                    context.SaveChanges();
                }

                MessageBox.Show("Đề thi đã được lưu thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                ExamSaved?.Invoke();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu đề thi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
