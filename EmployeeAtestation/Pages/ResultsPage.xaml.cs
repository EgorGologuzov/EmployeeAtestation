using EmployeeAtestation.ViewModels;
using System.Windows.Controls;

namespace EmployeeAtestation.Pages
{
    /// <summary>
    /// Логика взаимодействия для ResultsPage.xaml
    /// </summary>
    public partial class ResultsPage : Page
    {
        private TestResultListViewModel _viewModel;

        public ResultsPage()
        {
            InitializeComponent();

            _viewModel = new TestResultListViewModel();
            _viewModel.LoadResults.Execute(null);
            this.DataContext = _viewModel;
        }
    }
}
