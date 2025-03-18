using Project.Helpers;
using Project.Models;

namespace Project.ViewModels
{
    public class OnlineExamQuestionViewModel : BaseViewModel
    {
        public Question Question { get; set; }

        // Lưu đáp án được chọn cho câu hỏi này (OptionId)
        private int? _selectedOptionId;
        public int? SelectedOptionId
        {
            get => _selectedOptionId;
            set
            {
                _selectedOptionId = value;
                OnPropertyChanged(nameof(SelectedOptionId));
            }
        }
    }
}