using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace EmployeeAtestation.Controls
{
    /// <summary>
    /// Логика взаимодействия для ClearListView.xaml
    /// </summary>
    public partial class ClearListView : UserControl
    {
        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(ClearListView),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IList), typeof(ClearListView),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public ClearListView()
        {
            InitializeComponent();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name != nameof(ItemsSource))
            {
                return;
            }

            emptyView.Visibility = ItemsSource is null || ItemsSource.Count == 0 ? Visibility.Visible : Visibility.Collapsed;

            if (ItemsSource is INotifyCollectionChanged notifyCollection && ItemsSource is IList list)
            {
                notifyCollection.CollectionChanged += (s, e) =>
                {
                    emptyView.Visibility = list.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
                };
            }
        }
    }
}
