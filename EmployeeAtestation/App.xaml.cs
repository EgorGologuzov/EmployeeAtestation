using EmployeeAtestation.Pages;
using System.Configuration;
using System.Data;
using System.Windows;

namespace EmployeeAtestation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = new MainWindow();

            Navigation.Push(new AuthPage());

            MainWindow.Show();

            base.OnStartup(e);
        }
    }
}
