using System.ComponentModel;

namespace EmployeeAtestation.ViewModels
{
    public class ViewModelBase<TModel> : INotifyPropertyChanged
    {
        public readonly TModel Model;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ViewModelBase(TModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            Model = model;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
