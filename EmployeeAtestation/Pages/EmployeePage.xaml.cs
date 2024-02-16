using EmployeeAtestation.Data;
using EmployeeAtestation.Models;
using EmployeeAtestation.ViewModels;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace EmployeeAtestation.Pages
{
    /// <summary>
    /// Логика взаимодействия для EmployeePage.xaml
    /// </summary>
    public partial class EmployeePage : Page
    {
        private readonly EmployeeViewModel _viewModel;

        public EmployeePage(Employee employee)
        {
            InitializeComponent();

            _viewModel = new EmployeeViewModel(employee);
            this.DataContext = _viewModel;

            SetContent();
        }

        private async void SetContent()
        {
            try
            {
                Test test = await TestRepository.Get(_viewModel.Model.TestFileName);
                contentFrame.Navigate(new TestPage(new TestViewModel(test)));
            }
            catch
            {
                contentFrame.Navigate(new TestPage(new TestViewModel(new Test())));
                Navigation.ShowError("Не удалось загрузить файл вашего теста. Возможно вашы данные были неправильно заполнены, сообщите куратору об этой ошибке.");
            }
        }
    }
}
