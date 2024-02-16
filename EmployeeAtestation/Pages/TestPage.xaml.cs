using EmployeeAtestation.Models;
using EmployeeAtestation.ViewModels;
using System.Windows.Controls;
using System.Windows.Input;

namespace EmployeeAtestation.Pages
{
    /// <summary>
    /// Логика взаимодействия для TestPage.xaml
    /// </summary>
    public partial class TestPage : Page
    {
        public TestPage(TestViewModel test)
        {
            InitializeComponent();

            this.DataContext = test;
        }
    }
}
