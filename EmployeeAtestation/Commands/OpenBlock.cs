using EmployeeAtestation.Models;
using EmployeeAtestation.Pages;
using EmployeeAtestation.ViewModels;
using System.Windows.Input;

namespace EmployeeAtestation.Commands
{
    public class OpenBlock : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        public readonly BlockViewModel Block;

        public OpenBlock(BlockViewModel block)
        {
            if (block is null)
            {
                throw new ArgumentNullException(nameof(block));
            }

            Block = block;
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            Navigation.Push(new BlockPage(Block, BlockPageContent.Info));
        }
    }
}
