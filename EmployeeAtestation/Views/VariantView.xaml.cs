using EmployeeAtestation.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EmployeeAtestation.Views
{
    /// <summary>
    /// Логика взаимодействия для VariantView.xaml
    /// </summary>
    public partial class VariantView : UserControl
    {
        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(VariantViewModel), typeof(VariantView),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        
        private readonly Brush NonSelectedColor = (Brush)Application.Current.FindResource("Brush1");
        private readonly Brush SelectedColor = (Brush)Application.Current.FindResource("Brush3");

        public VariantViewModel ViewModel
        {
            get { return (VariantViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        public VariantView()
        {
            InitializeComponent();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name != nameof(ViewModel))
            {
                return;
            }

            this.DataContext = ViewModel;

            if (ViewModel is null)
            {
                return;
            }

            grid.Background = ViewModel.IsSelected ? SelectedColor : NonSelectedColor;

            ViewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ViewModel.IsSelected))
                {
                    grid.Background = ViewModel.IsSelected ? SelectedColor : NonSelectedColor;
                }
            };
        }

        private void grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel is not null)
            {
                ViewModel.IsSelected = !ViewModel.IsSelected;
            }
        }
    }
}
