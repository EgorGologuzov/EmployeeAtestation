namespace EmployeeAtestation.Commands
{
    public class ExitApplication : CommandBase
    {
        public override void Execute(object? parameter)
        {
            App.Current.Shutdown();
        }
    }
}
