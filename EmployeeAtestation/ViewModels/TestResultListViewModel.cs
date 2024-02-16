using EmployeeAtestation.Commands;
using EmployeeAtestation.Models;
using System.Windows.Input;

namespace EmployeeAtestation.ViewModels
{
    public class TestResultListViewModel : ViewModelBase<IList<TestResult>>
    {
        private DateTime _startDate = DateTime.Now - TimeSpan.FromDays(7);
        public DateTime StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(nameof(StartDate)); }
        }

        private DateTime _endDate = DateTime.Now + TimeSpan.FromDays(1);
        public DateTime EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(nameof(EndDate)); }
        }

        private bool _canLoad = true;
        public bool CanLoad
        {
            get => _canLoad;
            set { _canLoad = value; OnPropertyChanged(nameof(CanLoad)); }
        }

        public ViewModelList<TestResult, TestResultViewModel> Results { get; }

        public ICommand LoadResults { get; set; }

        public TestResultListViewModel() : base(new List<TestResult>())
        {
            Results = new(Model);
            LoadResults = new LoadResults(this);
        }
    }
}
