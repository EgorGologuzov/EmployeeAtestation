using System.Windows;
using System.Windows.Controls;

namespace EmployeeAtestation.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            Navigation.Push(new AddEmployeePage());
        }

        private void Results_Click(object sender, RoutedEventArgs e)
        {
            Navigation.Push(new ResultsPage());
        }
    }
}
