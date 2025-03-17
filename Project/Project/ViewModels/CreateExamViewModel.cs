using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Project.Helpers;   // Chứa BaseViewModel, RelayCommand
using Project.Models;    // Chứa model Exam, Question, Option

namespace Project.ViewModels
{
    public class CreateExamViewModel : BaseViewModel
    {
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

        public CreateExamViewModel()
        {
            // Khởi tạo thông tin kỳ thi
            ExamDate = DateTime.Now;
            Room = string.Empty;

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

            // Khởi tạo các lệnh
            AddQuestionCommand = new RelayCommand(o => AddQuestion());
            AddOptionCommand = new RelayCommand(o => AddOption(o));
            SelectImageCommand = new RelayCommand(o => SelectImage(o));
            SaveExamCommand = new RelayCommand(o => SaveExam());
        }

        // Cho phép giảng viên thêm câu hỏi (nếu số lượng hiện tại < 30)
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

        // Thêm một đáp án cho câu hỏi được truyền qua CommandParameter
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

        // Cho phép giảng viên chọn ảnh cho câu hỏi thông qua OpenFileDialog
        private void SelectImage(object parameter)
        {
            if (parameter is Question question)
            {
                var dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (dialog.ShowDialog() == true)
                {
                    question.ImagePath = dialog.FileName;
                    // Cập nhật lại danh sách để giao diện refresh (nếu cần)
                    OnPropertyChanged(nameof(Questions));
                }
            }
        }

        // Lưu đề thi vào cơ sở dữ liệu (Exam, sau đó Questions và Options)
        private void SaveExam()
        {
            // Kiểm tra thông tin bắt buộc
            if (string.IsNullOrWhiteSpace(Room))
            {
                MessageBox.Show("Vui lòng nhập phòng thi.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    // Tạo đối tượng Exam mới
                    Exam exam = new Exam
                    {
                        Date = DateOnly.FromDateTime(ExamDate),
                        Room = Room,
                        // Nếu có CourseId hay các thông tin khác, bạn có thể thêm vào đây.
                    };
                    context.Exams.Add(exam);
                    context.SaveChanges();

                    // Gán ExamId cho từng câu hỏi và lưu vào DB
                    foreach (var question in Questions)
                    {
                        question.ExamId = exam.ExamId;
                        context.Questions.Add(question);
                    }
                    context.SaveChanges();
                }

                MessageBox.Show("Đề thi đã được lưu thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu đề thi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
