using EmployeeAtestation.Pages;
using EmployeeAtestation.ViewModels;

namespace EmployeeAtestation.Commands
{
    public class MoveToNextBlock : CommandBase
    {
        public MoveToNextBlock(BlockViewModel block)
        {
            Block = block;
        }

        public BlockViewModel Block { get; }

        public override void Execute(object? parameter)
        {
            Navigation.PopTo<EmployeePage>();
            Navigation.Push(new BlockPage(Block.NextBlock, BlockPageContent.Info));
        }
    }
}
