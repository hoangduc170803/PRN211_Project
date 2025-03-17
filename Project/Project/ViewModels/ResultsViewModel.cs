
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
    public class ResultsViewModel : BaseViewModel
    {
        private ObservableCollection<Result> _results;
        public ObservableCollection<Result> Results
        {
            get => _results;
            set { _results = value; OnPropertyChanged(nameof(Results)); }
        }

        // Thuộc tính binding cho đối tượng Result mới
        private Result _newResult;
        public Result NewResult
        {
            get => _newResult;
            set { _newResult = value; OnPropertyChanged(nameof(NewResult)); }
        }

        public ICommand LoadResultsCommand { get; set; }
        public ICommand CreateNewResultCommand { get; set; }
        public ICommand SaveNewResultCommand { get; set; }
        public ICommand DeleteResultCommand { get; set; }

        public ResultsViewModel()
        {
            Results = new ObservableCollection<Result>();
            // Không khởi tạo NewResult trong constructor
            LoadResultsCommand = new RelayCommand(o => LoadResults());
            CreateNewResultCommand = new RelayCommand(o => CreateNewResult());
            SaveNewResultCommand = new RelayCommand(o => SaveNewResult(), o => CanSaveNewResult());
            DeleteResultCommand = new RelayCommand(o => DeleteResult(o));
        }

        public void LoadResults()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                var list = context.Results.ToList();
                Results.Clear();
                foreach (var result in list)
                    Results.Add(result);
            }
        }

        // Command tạo đối tượng Result mới để binding từ giao diện
        private void CreateNewResult()
        {
            NewResult = new Result();
        }

        // Kiểm tra điều kiện lưu (ví dụ: ExamID và UserID phải hợp lệ, Score có giá trị)
        public bool CanSaveNewResult()
        {
            return NewResult != null &&
                   NewResult.ExamId > 0 &&
                   NewResult.UserId > 0 &&
                   NewResult.Score >= 0;
        }

        public void SaveNewResult()
        {
            using (var context = new SafeDriveCertDbContext())
            {
                context.Results.Add(NewResult);
                context.SaveChanges();
                Results.Add(NewResult);
            }
            // Reset đối tượng sau khi lưu
            NewResult = null;
        }

        public void DeleteResult(object parameter)
        {
            if (parameter is Result result)
            {
                using (var context = new SafeDriveCertDbContext())
                {
                    context.Results.Remove(result);
                    context.SaveChanges();
                }
                Results.Remove(result);
            }
        }
    }
}
