using EmployeeAtestation.Commands;
using EmployeeAtestation.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace EmployeeAtestation.ViewModels
{
    public class EmployeeViewModel : ViewModelBase<Employee>
    {
        public readonly Dictionary<string, string> StationTestVariants = new()
        {
            { "Суши бар", "Суши бар.json" },
            { "Горячая кухня", "Горячая кухня.json" },
            { "Администратор", "Администратор.json" },
            { "Кассир", "Кассир.json" },
            { "Заготовщик", "Заготовщик.json" }
        };

        public string Firstname
        {
            get => Model.Firstname;
            set { Model.Firstname = value; OnPropertyChanged(nameof(Firstname)); }
        }

        public string Lastname
        {
            get => Model.Lastname;
            set { Model.Lastname = value; OnPropertyChanged(nameof(Lastname)); }
        }

        public string Station
        {
            get => Model.Station;
            set { Model.Station = value; OnPropertyChanged(nameof(Station)); }
        }

        private bool _canAdd = true;
        public bool CanAdd
        {
            get => _canAdd;
            set { _canAdd = value; OnPropertyChanged(nameof(CanAdd)); }
        }

        private Visibility _saveSecretsPanelVisibility = Visibility.Collapsed;
        public Visibility SaveSecretsPanelVisibility
        {
            get => _saveSecretsPanelVisibility;
            set { _saveSecretsPanelVisibility = value; OnPropertyChanged(nameof(SaveSecretsPanelVisibility)); }
        }

        private string _secrets;
        public string Secrets
        {
            get => _secrets;
            set { _secrets = value; OnPropertyChanged(nameof(Secrets)); }
        }

        public string[] Stations => StationTestVariants.Keys.ToArray();

        public string FullName => $"{Model.Firstname} {Model.Lastname}";

        public ICommand Add { get; set; }
        public ICommand SaveSecrets { get; set; }
        public ICommand CopySecrets { get; set; }

        public EmployeeViewModel(Employee model) : base(model)
        {
            Add = new AddEmployee(this);
            SaveSecrets = new SaveSecrets(this);
            CopySecrets = new CopySecrets(this);
        }
    }
}
