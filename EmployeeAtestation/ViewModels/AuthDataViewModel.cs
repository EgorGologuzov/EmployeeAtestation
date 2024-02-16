using EmployeeAtestation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeAtestation.ViewModels
{
    public class AuthDataViewModel : ViewModelBase<AuthData>
    {
        public string Code
        {
            get => Model.Code;
            set { Model.Code = value; OnPropertyChanged(nameof(Code)); }
        }

        public string Password
        {
            get => Model.Password;
            set { Model.Password = value; OnPropertyChanged(nameof(Password)); }
        }

        public AuthDataViewModel() : base(new AuthData())
        {
        }
    }
}
