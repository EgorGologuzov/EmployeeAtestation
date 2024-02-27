using EmployeeAtestation.Models;
using EmployeeAtestation.Utils;
using System.Security.Authentication;
using System.Text;

namespace EmployeeAtestation.Data
{
    public static class AuthSystem
    {
        private const string _adminCode = "000000";
        private const string _adminPasswordHash = "1f916cc3e376662c1775a2cc2c0d4ed8dc67b6189b90e442d9eac72e5976b245";
        private const string _sharedKeyHash = "b4f32e02dca2ae488a631871a8bb5c4313a2842e3287e523e5fc774ed1fbfeff";
        private const string _driveSecretsEncrypted = "Ljhwq0NXufHbDHmjyFL1CIcQtmBRIFTEgiCOKU89+OvXjTdvF4GTxQpOdu3tPqztjVY9EMwWai5UPdjzYm8Ez7rVAYrKT/czSOAGbe66sk+URT1cCe6S8Hwi90bxMvA+hUIIZ9vUVL2GAZBK/CcWUzCqrl15fnEkYwWjCMvJtDHTBZNDCQGxXwVN431zsKTJ8Rq+FIUv+dn1KdxbnJyb+dGuAO9Y55FydfP6GAhBkzr1bempSQ6pgez26cmz7X3XCIq5jF8NdnKu7tuorJCfT2c00rgt7QBOHJO9ylINRqz+bPE23edl0S1CgUV4mYftA+MCODyT7IeBd1xE2d368P8ihbv+zWAn70ISnqEwPk1lVXBXSWSe5EVqVcXPnRLsrFUpo0M9KDe6hAXat2Az3FrZBzeLjS7r2IxMpT5snxRnGPrB19XtWcOnhVJKMd5nuIjQbtbwyQljynuRSrO6CU9gBK1CiTHypsO9ZMHtAxJEkDUaQgvwmJnjh5O5onogbV5Qn+HuqacFXRWfVNTxtJX9dH+ILS8JowZst3eoSFA=";
        private const string _storedDataEncrypted = "fJcH3OFKpjMRXRc8ICYeWE6V1+KH5aLFuXPACbaxCjYnYGobjWlwhwjk+wVevxRk5YAyfGW5BkEm5YR4WSakj4baw2We7qBvIb2DbhofER9r/7hNWbDQB4LAYkACtTd80U6Odj2ch05Cbff6sOvw7GKISJZ2u4ppP3AQqeQI/XCFcY5w1opuYs7ZWlZzdv5g35wbKverakMvrvtl0PBMR8Ex77UsmCEKGaL8QtaXXSffpCzj5O/zvH09i/bwIJf6SzGcRkWz4AoubvNRNfW9VpbvGqN0MnWx0kfbFck/VuF178IEU1PWLBYicwKIWE8Z0f8u2ddK9l944wc8E6A/c1q/B9Glkdd4GmYYWsbnkiEx/BbMNa27Wj0mS0kSAbPYeaKy3W6j6tVdLOk1gdpQTQFrsLx/9h+BoYym6wnrXDwvkEo6ThvkKyeBE6PkqMJE3gUUUw1sO8VGnLDqY4GlYyXmwQjsg+6TWl2ciasDT85WKcnSuMFpgaOwqZ2i0eBw93IawQG70sIJjLA2BvNJwL23IXL3ZC7+m+qledSIr7S5J8ea7OqJ7xRJYTT3AnIJDhCLzBGfI9TlHkU8dvNPvhy4tsmcWop0yz0f9ermGBYJ7TLioe/ro+4xXDgT/Eop44u2whjlb7DtCmjVqqtkhz+YZKVQGM+2DRSxJ1kt4pmBtdcEK536vVl6bUUUTDBNI0yFX0J9zaZwWVylhX2CLT/v1yKJX7RD1foCbKGInN8=";

        public static Role? LoginedRole { get; private set; }
        public static Employee? LoginedEmployee { get; private set; }
        public static string? SharedKey { get; private set; }

        public static async Task Login(AuthData data)
        {
            string code = data.Code;
            string password = data.Password;

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(password) || password.Length != 12)
            {
                throw new AuthenticationException();
            }

            if (Drive.IsInitialized == false)
            {
                string sharedKey = password.Substring(6);

                if (sharedKey.ToSha256Hash() != _sharedKeyHash)
                {
                    throw new AuthenticationException();
                }

                SharedKey = sharedKey;
                sharedKey = Encryptor.ScaleKeyTo256Bit(sharedKey);
                string driveSecrets = Encryptor.DecryptString(sharedKey, _driveSecretsEncrypted);
                string storedData = Encryptor.DecryptString(sharedKey, _storedDataEncrypted);
                await Drive.Initialize(driveSecrets, storedData);
            }

            if (code == _adminCode && password.ToSha256Hash() == _adminPasswordHash)
            {
                LoginedRole = Role.Admin;
            }
            else
            {
                Employee employee = await EmployeeRepository.Get(code, password);

                if (employee is null)
                {
                    throw new AuthenticationException();
                }

                if (await ResultRepository.HasResult(employee.Code))
                {
                    throw new InvalidOperationException("Сотрудник уже прошел тест");
                }

                LoginedRole = Role.CertifiedEmployee;
                LoginedEmployee = employee;
            }
        }

        public static string GeneratePassword()
        {
            if (string.IsNullOrEmpty(SharedKey))
            {
                throw new ArgumentNullException("SharedKey in AuthSystem is null or empty");
            }

            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghigklmnopqrstuvwxyz0123456789";
            string individualKey = "";
            Random random = new Random();

            for (int i = 0; i < 6; i++)
            {
                individualKey += chars[random.Next(0, chars.Length)];
            }

            return individualKey + SharedKey;
        }
    }
}
