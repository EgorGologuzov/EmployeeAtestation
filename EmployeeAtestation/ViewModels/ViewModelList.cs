using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace EmployeeAtestation.ViewModels
{
    public class ViewModelList<TModel, TViewModel> : ObservableCollection<TViewModel> where TViewModel : ViewModelBase<TModel>
    {
        private IList<TModel> _models;

        public ViewModelList(IList<TModel> modelList)
        {
            if (modelList is null)
            {
                throw new ArgumentNullException(nameof(modelList));
            }

            _models = modelList;

            foreach (TModel model in _models)
            {
                this.Add((TViewModel)Activator.CreateInstance(typeof(TViewModel), model));
            }

            this.CollectionChanged += OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add: AddModels(e.NewItems); break;
                case NotifyCollectionChangedAction.Remove: RemoveModels(e.OldItems); break;
                case NotifyCollectionChangedAction.Reset: ResetModels(); break;
            }
        }

        private void AddModels(IList? newItems)
        {
            if (newItems is null)
            {
                return;
            }

            foreach (TViewModel item in newItems)
            {
                _models.Add(item.Model);
            }
        }

        private void RemoveModels(IList? oldItems)
        {
            if (oldItems is null)
            {
                return;
            }

            foreach (TViewModel item in oldItems)
            {
                if (_models.Contains(item.Model))
                {
                    _models.Remove(item.Model);
                }
            }
        }

        private void ResetModels()
        {
            _models.Clear();
        }
    }
}
