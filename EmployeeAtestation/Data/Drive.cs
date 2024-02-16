using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System.IO;
using System.Text;

namespace EmployeeAtestation.Data
{
    public static class Drive
    {
        public static DriveService Service { get; set; }
        public static bool IsInitialized => Service is not null;

        public static async Task Initialize(string secrets)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(secrets));

            var clientSecrets = GoogleClientSecrets.FromStream(stream).Secrets;
            var scopes = new string[] { DriveService.Scope.Drive };

            var credentials = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    clientSecrets: clientSecrets,
                    scopes: scopes,
                    user: "sushimanyat.tests@gmail.com",
                    taskCancellationToken: CancellationToken.None
                );

            Service = new DriveService(
                    new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credentials,
                        ApplicationName = "SushiManyait.EmployeeAtestation"
                    }
                );
        }

        public static async Task<Google.Apis.Drive.v3.Data.File?> GetFile(string fileId)
        {
            var request = Service.Files.Get(fileId);
            return await request.ExecuteAsync();
        }

        public static async Task<Google.Apis.Drive.v3.Data.File?> GetFile(string fileName, string? folderName = null)
        {
            if (Service is null)
            {
                throw new NullReferenceException("Drive.Service is null");
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var request = Service.Files.List();

            if (folderName is null)
            {
                request.Q = $"name='{fileName}'";
            }
            else
            {
                var folder = await GetFolder(folderName);

                if (folder is null)
                {
                    throw new FileNotFoundException($"Folder {folderName} not found on drive", folderName, null);
                }

                request.Q = $"'{folder.Id}' in parents and name = '{fileName}'";
            }

            var response = await request.ExecuteAsync();

            return response.Files.Count == 0 ? null : response.Files[0];
        }

        public static async Task<IList<Google.Apis.Drive.v3.Data.File>> GetAllFiles(string folderName)
        {
            var request = Service.Files.List();
            var folder = await GetFolder(folderName);

            if (folder is null)
            {
                throw new FileNotFoundException($"Folder {folderName} not found on drive", folderName, null);
            }

            request.Q = $"'{folder.Id}' in parents";

            var response = await request.ExecuteAsync();

            return response.Files.ToList();
        }

        public static async Task<IList<Google.Apis.Drive.v3.Data.File>> GetFilesBetween(string folderName, DateTime startDate, DateTime endDate)
        {
            var folder = await GetFolder(folderName);

            if (folder is null)
            {
                throw new FileNotFoundException($"Folder {folderName} not found on drive", folderName, null);
            }

            var request = Service.Files.List();
            request.Q = $"'{folder.Id}' in parents and modifiedTime > '{startDate.ToString("s")}' and modifiedTime < '{endDate.ToString("s")}'";

            var response = await request.ExecuteAsync();

            return response.Files.ToList();
        }

        public static async Task<Google.Apis.Drive.v3.Data.File?> GetFolder(string folderName)
        {
            var request = Service.Files.List();
            request.Q = $"name = '{folderName}' and mimeType = 'application/vnd.google-apps.folder'";
            var response = await request.ExecuteAsync();

            return response.Files.Count == 0 ? null : response.Files[0];
        }

        public static async Task DownloadFile(string fileId, string savePath)
        {
            using var stream = new FileStream(savePath, FileMode.Create, FileAccess.Write);
            await Service.Files.Get(fileId).DownloadAsync(stream);
        }

        public static async Task<string> ReadAllFromFile(string fileId)
        {
            var stream = new MemoryStream();
            await Service.Files.Get(fileId).DownloadAsync(stream);

            stream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream);

            return await reader.ReadToEndAsync();
        }

        public static async Task<string> ReadAllFromFile(string fileName, string? folderName = null)
        {
            var file = await GetFile(fileName, folderName);

            if (file is null)
            {
                throw new FileNotFoundException($"File {folderName ?? string.Empty}/{fileName} not found on drive", fileName, null);
            }

            var stream = new MemoryStream();
            await Service.Files.Get(file.Id).DownloadAsync(stream);

            stream.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(stream);

            return await reader.ReadToEndAsync();
        }

        public static async Task<Google.Apis.Drive.v3.Data.File> CreateFile(string fileName, string? folderName = null, string? content = null)
        {
            Google.Apis.Drive.v3.Data.File fileMetadata;

            if (folderName == null)
            {
                fileMetadata = new()
                {
                    Name = fileName,
                    MimeType = "application/json"
                };
            }
            else
            {
                var folderId = (await GetFolder(folderName)).Id;
                fileMetadata = new()
                {
                    Name = fileName,
                    Parents = new List<string>() { folderId },
                    MimeType = "application/json"
                };
            }

            var streamContent = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content ?? string.Empty));

            FilesResource.CreateMediaUpload request;
            using (streamContent)
            {
                request = Service.Files.Create(fileMetadata, streamContent, "application/json");
                await request.UploadAsync();
            }

            return request.ResponseBody;
        }
    }
}
