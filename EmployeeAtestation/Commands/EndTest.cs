using EmployeeAtestation.Pages;
using EmployeeAtestation.ViewModels;

namespace EmployeeAtestation.Commands
{
    public class EndTest : CommandBase
    {
        public readonly BlockViewModel Block;

        public EndTest(BlockViewModel block)
        {
            Block = block;
        }

        public override void Execute(object? parameter)
        {
            if (!Block.IsAllQuestionAnswered && Navigation.ShowYesNo("Вы ответели не на все вопросы. Уверены, что хотите продолжить?") == System.Windows.MessageBoxResult.No)
            {
                return;
            }

            Block.EndTime = DateTime.Now;
            Block.ResultVisibility = System.Windows.Visibility.Visible;
            Block.RefreshResult();

            if (Block.Result < Block.Model.PassingScore || Block.HasNextBlock == false)
            {
                Block.OnTestingCompleted();
            }
        }
    }
}
