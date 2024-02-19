using EmployeeAtestation.Data;
using Google.Apis.Util.Store;

namespace EmployeeAtestation.Utils
{
    public class InternalDataStore : IDataStore
    {
        private string _data;

        public InternalDataStore(string data)
        {
            _data = data;
        }

        public Task ClearAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync<T>(string key)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync<T>(string key)
        {
            if (key != DriveConfig.AccountEmail)
            {
                throw new NotImplementedException($"Google drive API client requested unexpected key '{key}'");
            }

            return _data.FromJson<T>();
        }

        public async Task StoreAsync<T>(string key, T value)
        {
            if (key != DriveConfig.AccountEmail)
            {
                throw new NotImplementedException($"Google drive API client requested unexpected key '{key}'");
            }

            _data = value.ToJson();
        }
    }
}
