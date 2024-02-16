using EmployeeAtestation.Models;
using System.Windows;

namespace EmployeeAtestation.ViewModels
{
    public class BlockResultViewModel : ViewModelBase<BlockResult>
    {
        public int Number
        {
            get => Model.Number;
            set { Model.Number = value; OnPropertyChanged(nameof(Number)); }
        }

        public string Title
        {
            get => Model.Title;
            set { Model.Title = value; OnPropertyChanged(nameof(Title)); }
        }

        public DateTime? StartTime
        {
            get => Model.StartTime;
            set { Model.StartTime = value; OnPropertyChanged(nameof(StartTime)); }
        }

        public DateTime? EndTime
        {
            get => Model.EndTime;
            set { Model.EndTime = value; OnPropertyChanged(nameof(EndTime)); }
        }

        public double Result
        {
            get => Model.Result;
            set { Model.Result = value; OnPropertyChanged(nameof(Result)); }
        }

        public Visibility TimesVisibility => StartTime is null || StartTime == default(DateTime) ? Visibility.Collapsed : Visibility.Visible;

        public TimeSpan? Time
        {
            get
            {
                try
                {
                    return EndTime - StartTime;
                }
                catch
                {
                    return null;
                }
            }
        }

        public BlockResultViewModel(BlockResult model) : base(model)
        {
        }
    }
}
