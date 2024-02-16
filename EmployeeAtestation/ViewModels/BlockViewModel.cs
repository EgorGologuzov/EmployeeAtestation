using EmployeeAtestation.Commands;
using EmployeeAtestation.Models;
using EmployeeAtestation.Pages;
using System.Windows;
using System.Windows.Input;

namespace EmployeeAtestation.ViewModels
{
    public class BlockViewModel : ViewModelBase<Block>
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

        private DateTime _startTime;
        public DateTime StartTime
        {
            get => _startTime;
            set { _startTime = value; OnPropertyChanged(nameof(StartTime)); }
        }

        private DateTime _endTime;
        public DateTime EndTime
        {
            get => _endTime;
            set { _endTime = value; OnPropertyChanged(nameof(EndTime)); }
        }

        private BlockViewModel _nextBlock;
        public BlockViewModel NextBlock
        {
            get => _nextBlock;
            set
            {
                _nextBlock = value;
                OnPropertyChanged(nameof(NextBlock));
                OnPropertyChanged(nameof(HasNextBlock));
                OnPropertyChanged(nameof(ToNextBlockButtonVisibility));
            }
        }

        private ViewModelList<string, FileIdViewModel> _files;
        public ViewModelList<string, FileIdViewModel> Files
        {
            get => _files;
            set { _files = value; OnPropertyChanged(nameof(Files)); }
        }

        private ViewModelList<Question, QuestionViewModel> _questions;
        public ViewModelList<Question, QuestionViewModel> Questions
        {
            get => _questions;
            set { _questions = value; OnPropertyChanged(nameof(Questions)); }
        }

        private bool _isEnabled = false;
        public bool IsEnabled
        {
            get => _isEnabled;
            set { _isEnabled = value; OnPropertyChanged(nameof(IsEnabled)); }
        }

        private double _result;
        public double Result
        {
            get => _result;
            private set
            {
                _result = value;
                OnPropertyChanged(nameof(Result));
                OnPropertyChanged(nameof(SuccessButtonVisibility));
                OnPropertyChanged(nameof(FailedButtonVisibility));
            }
        }

        public Visibility SuccessButtonVisibility => Result >= Model.PassingScore ? Visibility.Visible : Visibility.Collapsed;
        public Visibility FailedButtonVisibility => Result < Model.PassingScore ? Visibility.Visible : Visibility.Collapsed;

        private Visibility _resultVisibility = Visibility.Collapsed;
        public Visibility ResultVisibility
        {
            get => _resultVisibility;
            set { _resultVisibility = value; OnPropertyChanged(nameof(ResultVisibility)); }
        }

        public bool IsAllQuestionAnswered
        {
            get
            {
                foreach (var question in this.Questions)
                {
                    if (question.Model.EmployeeAnswers.Count == 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public bool HasNextBlock => NextBlock is not null;

        public Visibility ToNextBlockButtonVisibility => HasNextBlock ? Visibility.Visible : Visibility.Hidden;

        public event EventHandler TestingCompleted;

        public ICommand Open { get; set; }
        public ICommand StartTest { get; set; }
        public ICommand EndTest { get; set; }
        public ICommand ToEmployeePage { get; set; }
        public ICommand MoveToNextBlock { get; set; }
        public ICommand Shutdown { get; set; }

        public BlockViewModel(Block model) : base(model)
        {
            Model.Files ??= new List<string>();
            Files = new(Model.Files);
            Model.Questions ??= new List<Question>();
            Questions = new(Model.Questions);
            Open = new OpenBlock(this);
            StartTest = new StartTest(this);
            EndTest = new EndTest(this);
            ToEmployeePage = new PopToEmployeePage();
            Shutdown = new ExitApplication();
            MoveToNextBlock = new MoveToNextBlock(this);
            RefreshResult();
        }

        public void RefreshResult()
        {
            double employeeScore = 0;
            double maxScore = 0;

            foreach (var question in this.Questions)
            {
                maxScore += question.Model.Score;

                if (question.IsAnswerCorrect)
                {
                    employeeScore += question.Model.Score;
                }
            }

            if (maxScore == 0)
            {
                Result = 0;
            }
            else
            {
                Result = (employeeScore / maxScore) * 100;
            }
        }

        public void OnTestingCompleted()
        {
            TestingCompleted?.Invoke(this, new EventArgs());
        }
    }
}
