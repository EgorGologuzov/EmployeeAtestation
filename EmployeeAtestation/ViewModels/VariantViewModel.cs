namespace EmployeeAtestation.ViewModels
{
    public class VariantViewModel : ViewModelBase<string>
    {
        private string _prefix;
        public string Prefix
        {
            get => _prefix;
            set { _prefix = value; OnPropertyChanged(nameof(Prefix)); }
        }

        public string Text => Model;

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(nameof(IsSelected)); }
        }

        public VariantViewModel(string model) : base(model)
        {
        }
    }
}
