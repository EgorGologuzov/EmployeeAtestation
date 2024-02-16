using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmployeeAtestation.Controls
{
    /// <summary>
    /// Логика взаимодействия для RegularButton.xaml
    /// </summary>
    public partial class RegularButton : UserControl
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(RegularButton),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));



        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(RegularButton), 
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public event RoutedEventHandler Click;

        public RegularButton()
        {
            InitializeComponent();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property.Name != nameof(this.IsEnabled))
            {
                return;
            }

            if (this.IsEnabled == false)
            {
                border.Background = (Brush)Application.Current.FindResource("Disabled");
            }
            else if (this.IsMouseOver)
            {
                border.Background = (Brush)Application.Current.FindResource("Brush2");
            }
            else
            {
                border.Background = (Brush)Application.Current.FindResource("Brush3");
            }
        }

        private void button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (this.IsEnabled)
            {
                border.Background = (Brush)Application.Current.FindResource("Brush2");
            }
        }

        private void button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.IsEnabled)
            {
                border.Background = (Brush)Application.Current.FindResource("Brush3");
            }
        }

        private void button_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.IsEnabled)
            {
                Click?.Invoke(this, e);

                if (Command is not null && Command.CanExecute(null))
                {
                    Command.Execute(null);
                }
            }
        }
    }
}
