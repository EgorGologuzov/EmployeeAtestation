using EmployeeAtestation.ViewModels;
using System.Windows.Controls;

namespace EmployeeAtestation.Pages
{
    public enum BlockPageContent
    {
        Info, Questions
    }

    /// <summary>
    /// Логика взаимодействия для BlockPage.xaml
    /// </summary>
    public partial class BlockPage : Page
    {
        public BlockPage(BlockViewModel block, BlockPageContent content)
        {
            InitializeComponent();

            this.DataContext = block;

            switch (content)
            {
                case BlockPageContent.Info:
                    contentFrame.Navigate(new BlockInfoPage(block));
                    break;
                case BlockPageContent.Questions:
                    block.StartTime = DateTime.Now;
                    contentFrame.Navigate(new QuestionPage(block));
                    header.BackButtonVisibility = System.Windows.Visibility.Collapsed;
                    break;
            }
            
        }
    }
}
