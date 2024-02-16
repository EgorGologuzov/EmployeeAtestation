using EmployeeAtestation.ViewModels;
using System.Diagnostics;
using System.Windows.Controls;

namespace EmployeeAtestation.Pages
{
    /// <summary>
    /// Логика взаимодействия для BlockInfoPage.xaml
    /// </summary>
    public partial class BlockInfoPage : Page
    {
        public BlockInfoPage(BlockViewModel block)
        {
            InitializeComponent();

            this.DataContext = block;
            LoadFilesId(block.Files);
        }

        private async void LoadFilesId(ViewModelList<string, FileIdViewModel> files)
        {
            try
            {
                var tasks = files.Select(f => f.LoadFileId());
                await Task.WhenAll(tasks);
            }
            catch { }
        }
    }
}
