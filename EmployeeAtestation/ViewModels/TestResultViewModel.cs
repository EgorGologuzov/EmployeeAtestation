using EmployeeAtestation.Models;

namespace EmployeeAtestation.ViewModels
{
    public class TestResultViewModel : ViewModelBase<TestResult>
    {
        public string EmployeeCode
        {
            get => Model.Employee.Code;
            set { Model.Employee.Code = value; OnPropertyChanged(nameof(EmployeeCode)); }
        }

        public string EmployeeName => $"{Model.Employee.Firstname} {Model.Employee.Lastname}";

        public string EmployeeStation => Model.Employee.Station;

        public ViewModelList<BlockResult, BlockResultViewModel> Blocks { get; set; }

        private double _totalResult;
        public double TotalResult
        {
            get => _totalResult;
            set { _totalResult = value; OnPropertyChanged(nameof(TotalResult)); }
        }

        public TestResultViewModel(TestResult model) : base(model)
        {
            Model.Blocks ??= new List<BlockResult>();
            Blocks = new(Model.Blocks);
            RefreshTotalResult();
        }

        public void RefreshTotalResult()
        {
            double sum = Blocks.Sum(b => b.Result);
            double maxSum = Blocks.Count * 100;

            TotalResult = (sum / maxSum) * 100;
        }
    }
}
