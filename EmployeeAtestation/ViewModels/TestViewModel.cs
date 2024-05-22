using EmployeeAtestation.Data;
using EmployeeAtestation.Models;
using EmployeeAtestation.Pages;
using System.CodeDom;
using System.Windows;
using System.Windows.Controls;

namespace EmployeeAtestation.ViewModels
{
    public class TestViewModel : ViewModelBase<Test>
    {
        public string Title
        {
            get => Model.Title;
            set { Model.Title = value; OnPropertyChanged(nameof(Title)); }
        }

        private ViewModelList<Block, BlockViewModel> _blocks;
        public ViewModelList<Block, BlockViewModel> Blocks
        {
            get => _blocks;
            set { _blocks = value; OnPropertyChanged(nameof(Blocks)); }
        }

        private bool _isTestFinished = false;
        public bool IsTestFinished
        {
            get => _isTestFinished;
            set
            {
                _isTestFinished = value; 
                OnPropertyChanged(nameof(IsTestFinished));
                OnPropertyChanged(nameof(TestFinishedLabelVisibility));
            }
        }

        public Visibility TestFinishedLabelVisibility => IsTestFinished ? Visibility.Visible : Visibility.Collapsed;

        public TestViewModel(Test model) : base(model)
        {
            Model.Blocks ??= new List<Block>();
            Blocks = new(Model.Blocks);
            RefrashBlocksEnabled();

            foreach (var block in Blocks)
            {
                block.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(block.Result))
                    {
                        RefrashBlocksEnabled();
                    }
                };

                block.TestingCompleted += async (s, e) =>
                {
                    var sender = (BlockViewModel)s;

                    if (sender.HasNextBlock == false)
                    {
                        IsTestFinished = true;
                    }

                    while (true)
                    {
                        try
                        {
                            var employee = AuthSystem.LoginedEmployee;
                            var result = CollectTestResult();
                            result.Employee = employee;

                            await ResultRepository.SendResult(employee.Code, result);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Navigation.ShowError(ex.Message);
                        }
                    }
                };
            }
        }

        public void RefrashBlocksEnabled()
        {
            if (Blocks is null || Blocks.Count == 0)
            {
                return;
            }

            BlockViewModel previous = Blocks[0];
            previous.IsEnabled = true;

            for (int i = 1; i < Blocks.Count; i++)
            {
                var block = Blocks[i];
                previous.NextBlock = block;

                if (previous.Result < previous.Model.PassingScore)
                {
                    break;
                }

                block.IsEnabled = true;
                previous = block;
            }
        }

        public TestResult CollectTestResult()
        {
            TestResult result = new();

            foreach (var block in Blocks)
            {
                BlockResult blockResult = new()
                {
                    Number = block.Number,
                    StartTime = block.StartTime,
                    EndTime = block.EndTime,
                    Result = Math.Round(block.Result, 2),
                    Title = block.Title
                };

                result.Blocks.Add(blockResult);

                foreach (var question in block.Questions)
                {
                    blockResult.Answers.Add(new QuestionResult
                    {
                        Number = question.Number,
                        Answers = question.Model.EmployeeAnswers,
                        IsCorrect = question.IsAnswerCorrect
                    });
                }
            }

            return result;
        }
    }
}
