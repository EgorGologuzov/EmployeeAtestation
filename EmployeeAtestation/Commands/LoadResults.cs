using EmployeeAtestation.Data;
using EmployeeAtestation.Pages;
using EmployeeAtestation.ViewModels;

namespace EmployeeAtestation.Commands
{
    public class LoadResults : CommandBase
    {
        private readonly TestResultListViewModel _viewModel;

        public LoadResults(TestResultListViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public override async void Execute(object? parameter)
        {
            try
            {
                _viewModel.CanLoad = false;

                var results = await ResultRepository.GetBetween(_viewModel.StartDate, _viewModel.EndDate);
                _viewModel.Results.Clear();

                foreach (var result in results)
                {
                    _viewModel.Results.Add(new TestResultViewModel(result));
                }
            }
            catch (Exception ex)
            {
                Navigation.ShowError($"Не удалось загрузить результаты: {ex.Message}");
            }
            finally
            {
                _viewModel.CanLoad = true;
            }
        }
    }
}
