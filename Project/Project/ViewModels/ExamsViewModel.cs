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
    public class ExamsViewModel : BaseViewModel
    {
        private ObservableCollection<Exam> _exams;
        public ObservableCollection<Exam> Exams
        {
            get => _exams;
            set { _exams = value; OnPropertyChanged(nameof(Exams)); }
        }

        // Thuộc tính binding cho đối tượng Exam mới; không khởi tạo mặc định trong constructor
        private Exam _newExam;
        public Exam NewExam
        {
            get => _newExam;
            set { _newExam = value; OnPropertyChanged(nameof(NewExam)); }
        }

        public ICommand LoadExamsCommand { get; set; }
        public ICommand CreateNewExamCommand { get; set; }
        public ICommand SaveNewExamCommand { get; set; }
        public ICommand DeleteExamCommand { get; set; }

        public ExamsViewModel()
        {
            Exams = new ObservableCollection<Exam>();
            // Không khởi tạo NewExam ở đây, chờ người dùng nhấn "New Exam"
            LoadExamsCommand = new RelayCommand(o => LoadExams());
            CreateNewExamCommand = new RelayCommand(o => CreateNewExam());
            SaveNewExamCommand = new RelayCommand(o => SaveNewExam(), o => CanSaveNewExam());
            DeleteExamCommand = new RelayCommand(o => DeleteExam(o));
        }

        public void LoadExams()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                var list = context.Exams.ToList();
                Exams.Clear();
                foreach (var exam in list)
                    Exams.Add(exam);
            }
        }

        // Command tạo đối tượng Exam mới để binding từ giao diện
        private void CreateNewExam()
        {
            NewExam = new Exam();
        }

        // Kiểm tra điều kiện lưu (ví dụ: CourseID > 0, Date được chọn và Room không rỗng)
        public bool CanSaveNewExam()
        {
            return NewExam != null &&
                   NewExam.CourseId > 0 &&
                   NewExam.ExamDate != DateOnly.FromDateTime(default(DateTime)) &&
                   !string.IsNullOrWhiteSpace(NewExam.Room);
        }

        public void SaveNewExam()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                context.Exams.Add(NewExam);
                context.SaveChanges();
                Exams.Add(NewExam);
            }
            // Sau khi lưu, reset đối tượng để sẵn sàng cho lần nhập mới
            NewExam = null;
        }

        public void DeleteExam(object parameter)
        {
            if (parameter is Exam exam)
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    context.Exams.Remove(exam);
                    context.SaveChanges();
                }
                Exams.Remove(exam);
            }
        }
    }
}
