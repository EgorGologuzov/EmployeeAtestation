using EmployeeAtestation.Data;
using EmployeeAtestation.Models;
using EmployeeAtestation.Pages;
using EmployeeAtestation.Utils;
using EmployeeAtestation.ViewModels;

namespace EmployeeAtestation.Commands
{
    public class AddEmployee : CommandBase
    {
        private readonly EmployeeViewModel _employee;

        public AddEmployee(EmployeeViewModel employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            _employee = employee;
        }

        public override async void Execute(object? parameter)
        {
            if (string.IsNullOrEmpty(_employee.Firstname))
            {
                Navigation.ShowError($"Имя не заполнено");
                return;
            }
            if (string.IsNullOrEmpty(_employee.Lastname))
            {
                Navigation.ShowError($"Фамилия не заполнена");
                return;
            }
            if (string.IsNullOrEmpty(_employee.Station))
            {
                Navigation.ShowError($"Станция не выбрана");
                return;
            }

            try
            {
                _employee.CanAdd = false;

                string password = AuthSystem.GeneratePassword();
                _employee.Model.PasswordHash = password.ToSha256Hash();
                _employee.Model.TestFileName = _employee.StationTestVariants[_employee.Station];

                var result = await EmployeeRepository.Create(_employee.Model);

                _employee.Secrets = $"Код: {result.Code}; Пароль: {password}";
                _employee.SaveSecretsPanelVisibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                Navigation.ShowError($"Не удалось добавить сотрудника: {ex.Message}");
            }
            finally
            {
                _employee.CanAdd = true;
            }
        }
    }
}
