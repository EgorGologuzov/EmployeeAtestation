using EmployeeAtestation.Data;
using EmployeeAtestation.Pages;
using System.ComponentModel;
using System.Windows.Input;

namespace EmployeeAtestation.Commands
{
    internal class DownloadFile : ICommand, INotifyPropertyChanged
    {
        public readonly string File;

        private bool _isDownloadFree = true;
        public bool IsDownloadFree
        {
            get => _isDownloadFree;
            set { _isDownloadFree = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsDownloadFree))); }
        }
        public event EventHandler? CanExecuteChanged;
        public event PropertyChangedEventHandler? PropertyChanged;
        public DownloadFile(string file)
        {
            File = file;
        }

        public bool CanExecute(object? parameter) => true;

        public async void Execute(object? parameter)
        {
            IsDownloadFree = false;

            try
            {
                var file = await Drive.GetFile(File, DriveConfig.MaterialsFolder);
                Navigation.SaveFileDialog.FileName = File;

                bool? result = Navigation.SaveFileDialog.ShowDialog();

                if (result == true)
                {
                    await Drive.DownloadFile(file.Id, Navigation.SaveFileDialog.FileName);
                }
            }
            catch
            {
                Navigation.ShowError("Не удалось загрузить файл");
            }

            IsDownloadFree = true;
        }
    }
}
