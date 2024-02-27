using EmployeeAtestation.Pages;
using EmployeeAtestation.ViewModels;
using System.Windows;

namespace EmployeeAtestation.Commands
{
    public class RestartBlock : CommandBase
    {
        private readonly BlockViewModel _block;

        public RestartBlock(BlockViewModel block)
        {
            _block = block;
        }

        public override void Execute(object? parameter)
        {
            _block.ResultVisibility = Visibility.Collapsed;

            foreach (var question in _block.Questions)
            {
                question.ResetAnswers();
            }

            Navigation.PopTo<EmployeePage>();
        }
    }
}
