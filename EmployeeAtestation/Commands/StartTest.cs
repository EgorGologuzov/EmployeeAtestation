using EmployeeAtestation.Pages;
using EmployeeAtestation.ViewModels;
using System.Windows.Input;

namespace EmployeeAtestation.Commands
{
    public class StartTest : ICommand
    {
        private readonly BlockViewModel _block;

        public event EventHandler? CanExecuteChanged;

        public StartTest(BlockViewModel block)
        {
            if (block is null)
            {
                throw new ArgumentNullException(nameof(block));
            }

            _block = block;
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            Navigation.Push(new BlockPage(_block, BlockPageContent.Questions));
        }
    }
}
