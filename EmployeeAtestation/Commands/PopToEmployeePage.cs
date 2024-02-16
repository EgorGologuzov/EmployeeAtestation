using EmployeeAtestation.Pages;
using System.Windows.Controls;

namespace EmployeeAtestation.Commands
{
    public class PopToEmployeePage : CommandBase
    {
        public override void Execute(object? parameter)
        {
            Navigation.PopTo<EmployeePage>();
        }
    }
}
