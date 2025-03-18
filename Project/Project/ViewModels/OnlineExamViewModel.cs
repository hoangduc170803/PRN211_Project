using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using Project.Helpers;
using Project.Models;
using Project.Views;

namespace Project.ViewModels
{
    public class OnlineExamViewModel : BaseViewModel
    {
        public int ExamId { get; set; }
        public int UserId { get; set; }
        public ObservableCollection<OnlineExamQuestionViewModel> Questions { get; set; }
        public ICommand SubmitExamCommand { get; set; }

        public OnlineExamViewModel(int examId, int userId)
        {
            ExamId = examId;
            UserId = userId;
            Questions = new ObservableCollection<OnlineExamQuestionViewModel>();
            SubmitExamCommand = new RelayCommand(o => SubmitExam());
            LoadExamQuestions();
        }

        private void LoadExamQuestions()
        {
            try
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    var exam = context.Exams
                        .Include(e => e.Questions)
                            .ThenInclude(q => q.Options)
                        .FirstOrDefault(e => e.ExamId == ExamId);
                    if (exam != null && exam.Questions.Any())
                    {
                        Questions.Clear();
                        foreach (var question in exam.Questions)
                        {
                            Questions.Add(new OnlineExamQuestionViewModel { Question = question });
                        }
                    }
                    else
                    {
                        MessageBox.Show("Kỳ thi không có câu hỏi.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        // Option: Không mở window hoặc đóng cửa sổ hiện tại
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load câu hỏi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void SubmitExam()
        {
            if (Questions.Count == 0)
            {
                MessageBox.Show("Không có câu hỏi nào để chấm.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int correct = 0;
            foreach (var qvm in Questions)
            {
                var correctOption = qvm.Question.Options.FirstOrDefault(o => o.IsCorrect);
                System.Diagnostics.Debug.WriteLine($"QuestionId: {qvm.Question.QuestionId}, SelectedOptionId: {qvm.SelectedOptionId}, CorrectOptionId: {correctOption?.OptionId}");
                if (correctOption != null && qvm.SelectedOptionId.HasValue && qvm.SelectedOptionId.Value == correctOption.OptionId)
                {
                    correct++;
                }
            }

            decimal score = (decimal)correct / Questions.Count * 100;
            MessageBox.Show($"Bạn đạt được {score:0.00} điểm.", "Kết quả", MessageBoxButton.OK, MessageBoxImage.Information);

            using (var context = new SafeDriveCertDbContext())
            {
                // Nếu chưa có kết quả, lưu kết quả thi
                var existingResult = context.Results.FirstOrDefault(r => r.UserId == UserId && r.ExamId == ExamId);
                if (existingResult == null)
                {
                    var result = new Result
                    {
                        UserId = UserId,
                        ExamId = ExamId,
                        Score = score,
                        PassStatus = score >= 70
                    };
                    context.Results.Add(result);
                    context.SaveChanges();
                }

                // Nếu đạt (score >= 70), cập nhật đăng ký và cấp chứng chỉ
                if (score >= 70)
                {
                    // Lấy kỳ thi hiện tại để truy xuất CourseId
                    var exam = context.Exams.FirstOrDefault(e => e.ExamId == ExamId);
                    if (exam != null)
                    {
                        var registration = context.Registrations.FirstOrDefault(r => r.UserId == UserId && r.CourseId == exam.CourseId);
                        if (registration != null)
                        {
                            registration.Status = "approved";
                        }
                        // Nếu chưa có chứng chỉ, tạo chứng chỉ
                        var certificate = context.Certificates.FirstOrDefault(c => c.UserId == UserId && c.ExamId == ExamId);
                        if (certificate == null)
                        {
                            certificate = new Certificate
                            {
                                UserId = UserId,
                                ExamId = ExamId,
                                CertificateCode = "CERT" + DateTime.Now.Ticks,
                                IssuedDate = DateOnly.FromDateTime(DateTime.Now),
                                ExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddYears(15))
                            };
                            context.Certificates.Add(certificate);
                        }
                        context.SaveChanges();
                    }
                }
            }

            // Sau khi nộp bài, đóng cửa sổ thi để quay lại StudentWindow
            Application.Current.Windows.OfType<OnlineExamWindow>().FirstOrDefault()?.Close();
        }
    }
    }

