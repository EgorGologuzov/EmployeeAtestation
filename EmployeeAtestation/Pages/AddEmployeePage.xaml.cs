using EmployeeAtestation.Models;
using EmployeeAtestation.ViewModels;
using System.Windows.Controls;

namespace EmployeeAtestation.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEmployeePage.xaml
    /// </summary>
    public partial class AddEmployeePage : Page
    {
        public AddEmployeePage()
        {
            InitializeComponent();

            this.DataContext = new EmployeeViewModel(new Employee());
        }
    }
}
