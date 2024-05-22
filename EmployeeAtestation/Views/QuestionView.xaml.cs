using EmployeeAtestation.Controls;
using EmployeeAtestation.Models;
using EmployeeAtestation.ViewModels;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace EmployeeAtestation.Views
{
    /// <summary>
    /// Логика взаимодействия для QuestionView.xaml
    /// </summary>
    public partial class QuestionView : UserControl
    {
        public QuestionViewModel ViewModel
        {
            get { return (QuestionViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(QuestionViewModel), typeof(QuestionView),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public QuestionView()
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

            if (ViewModel.Variants is not null)
            {
                foreach (var item in ViewModel.Variants)
                {
                    item.PropertyChanged += OnVariantPropertyChanged;
                }
            }
        }

        private void OnVariantPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = (VariantViewModel)sender;

            if (e.PropertyName != nameof(item.IsSelected))
            {
                return;
            }

            if (item.IsSelected == false)
            {
                ViewModel.Model.EmployeeAnswers.Remove(item.Text);
                return;
            }

            switch (ViewModel.Type)
            {
                case QuestionType.One:

                    ViewModel.Model.EmployeeAnswers.Clear();
                    ViewModel.Model.EmployeeAnswers.Add(item.Text);

                    foreach (var variant in ViewModel.Variants)
                    {
                        if (!variant.Equals(sender))
                        {
                            variant.IsSelected = false;
                        }
                    }

                    break;

                case QuestionType.Any:

                    if (ViewModel.Model.EmployeeAnswers.Contains(item.Text) == false)
                    {
                        ViewModel.Model.EmployeeAnswers.Add(item.Text);
                    }

                    break;
            }
        }
    }
}
