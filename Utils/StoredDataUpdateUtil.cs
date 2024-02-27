using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Text;

namespace Utils
{
    internal static class StoredDataUpdateUtil
    {
        public static DriveService Service;
        const string _secrets = "{\"installed\":{\"client_id\":\"310273913070-v3qbqm0epurtl57r37svqru63opor0lq.apps.googleusercontent.com\",\"project_id\":\"employeeatestation\",\"auth_uri\":\"https://accounts.google.com/o/oauth2/auth\",\"token_uri\":\"https://oauth2.googleapis.com/token\",\"auth_provider_x509_cert_url\":\"https://www.googleapis.com/oauth2/v1/certs\",\"client_secret\":\"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\",\"redirect_uris\":[\"http://localhost\"]}}";
        const string _fileDataStorePath = "C:\\Users\\Egor\\Desktop\\datastore";
        const string _accountEmail = "sushimanyat.tests@gmail.com";
        const string _applicationName = "SushiManyait.EmployeeAtestation";

        public static async Task<string> UpdateStoredData()
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(_secrets));

            var clientSecrets = GoogleClientSecrets.FromStream(stream).Secrets;
            var scopes = new string[] { DriveService.Scope.Drive };
            var dataStore = new FileDataStore(_fileDataStorePath);

            var credentials = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    clientSecrets: clientSecrets,
                    scopes: scopes,
                    user: _accountEmail,
                    taskCancellationToken: CancellationToken.None,
                    dataStore: dataStore
                );

            Service = new DriveService(
                    new BaseClientService.Initializer()
                    {
                        HttpClientInitializer = credentials,
                        ApplicationName = _applicationName
                    }
                );

            var data = await dataStore.GetAsync<TokenResponse>(_accountEmail);

            return data.ToJson();
        }
    }
}
