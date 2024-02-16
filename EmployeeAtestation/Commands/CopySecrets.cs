using EmployeeAtestation.ViewModels;
using System.Windows;

namespace EmployeeAtestation.Commands
{
    public class CopySecrets : CommandBase
    {
        private readonly EmployeeViewModel _employee;

        public CopySecrets(EmployeeViewModel employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            _employee = employee;
        }

        public override void Execute(object? parameter)
        {
            Clipboard.SetText(_employee.Secrets);
        }
    }
}
