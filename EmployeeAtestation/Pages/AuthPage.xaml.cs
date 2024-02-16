using EmployeeAtestation.Data;
using EmployeeAtestation.Models;
using EmployeeAtestation.ViewModels;
using System.Security.Authentication;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EmployeeAtestation.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private AuthDataViewModel _view;

        public AuthPage()
        {
            InitializeComponent();

            _view = new AuthDataViewModel();
            this.DataContext = _view;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await Login();
        }

        private async Task Login()
        {
            loginButton.IsEnabled = false;

            try
            {
                await AuthSystem.Login(_view.Model);

                if (AuthSystem.LoginedRole == Role.CertifiedEmployee)
                {
                    Navigation.Push(new EmployeePage(AuthSystem.LoginedEmployee));
                }
                else
                {
                    Navigation.Push(new AdminPage());
                }
            }
            catch (AuthenticationException)
            {
                Navigation.ShowError($"Неверный код или пароль: {_view.Code} : {_view.Password}");
            }
            catch (InvalidOperationException)
            {
                Navigation.ShowError("Вы уже прошли тестирование");
            }
            catch (Exception ex)
            {
                Navigation.ShowError($"Не удалось подключиться к диску: {ex.Message}");
            }
            finally
            {
                loginButton.IsEnabled = true;
            }
        }

        private async void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await Login();
            }
        }
    }
}
