using EmployeeAtestation.Commands;
using EmployeeAtestation.Data;
using EmployeeAtestation.Pages;
using System.CodeDom;
using System.IO;
using System.Windows.Input;

namespace EmployeeAtestation.ViewModels
{
    public class FileIdViewModel : ViewModelBase<string>
    {
        public string FileName => Model;

        private string _fileId;
        public string FileId
        {
            get => _fileId;
            private set
            {
                _fileId = value; 
                OnPropertyChanged(nameof(FileId));
                OnPropertyChanged(nameof(Url));
            }
        }

        public string Url => $"https://drive.google.com/file/d/{FileId}/view?usp=sharing";

        public ICommand Download { get; set; }

        public FileIdViewModel(string model) : base(model)
        {
            Download = new DownloadFile(Model);
        }

        public async Task LoadFileId()
        {
            try
            {
                var file = await Drive.GetFile(FileName, DriveConfig.MaterialsFolder);
                FileId = file.Id;
            }
            catch
            {
                throw new FileNotFoundException();
            }

        }
    }
}
