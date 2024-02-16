using EmployeeAtestation.Pages;
using EmployeeAtestation.ViewModels;
using System.IO;
using System.Windows;

namespace EmployeeAtestation.Commands
{
    public class SaveSecrets : CommandBase
    {
        private readonly EmployeeViewModel _employee;

        public SaveSecrets(EmployeeViewModel employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            _employee = employee;
        }

        public override void Execute(object? parameter)
        {
            Navigation.SaveFileDialog.FileName = $"secrets-{_employee.Model.Code}-{_employee.Firstname}-{_employee.Lastname}.txt";
            bool? result = Navigation.SaveFileDialog.ShowDialog();

            if (result == true)
            {
                File.WriteAllText(Navigation.SaveFileDialog.FileName, _employee.Secrets);
            }
        }
    }
}
