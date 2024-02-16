using System.Windows.Controls;

namespace EmployeeAtestation.Controls
{
    /// <summary>
    /// Логика взаимодействия для ClearScrollViewer.xaml
    /// </summary>
    public partial class ClearScrollViewer : ScrollViewer
    {
        public ClearScrollViewer()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}
