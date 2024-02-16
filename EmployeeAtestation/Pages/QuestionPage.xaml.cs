using EmployeeAtestation.Models;
using EmployeeAtestation.ViewModels;
using System.Windows.Controls;

namespace EmployeeAtestation.Pages
{
    /// <summary>
    /// Логика взаимодействия для QuestionPage.xaml
    /// </summary>
    public partial class QuestionPage : Page
    {
        public QuestionPage(BlockViewModel block)
        {
            InitializeComponent();

            this.DataContext = block;
        }
    }
}
