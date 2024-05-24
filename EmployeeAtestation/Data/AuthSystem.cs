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
        private const string _storedDataEncrypted = "fJcH3OFKpjMRXRc8ICYeWDUuPUiKpyTUnfBxAji7ByVYFoS2cARzqHH5XFfUFCDjw+z7ZhHYuQ87S/xk+uDA9IRqzul7ARkaJkJM4vgJRqVm2W8Yqb2jwTqcd72Ui7lAV7AAyedVT0y8nvaCjyplnZPsqsUsgJKmZms26EAV0h+rmxC8ErLGRycpSZZTA1YU3h/QHu1/ttgcLpFNPx+KC1HEgLaC0N/AdUre3ugxOYJMTCb+iNjxF9Y81wSN3q+wUyXJ6VOpfX4rLXKsjs/1PNeXRK5Sg5yN5NSpYwt+uNhoI5FtXM0CpNt8+Wo9lwbY+ajVmmnIwZ52+JukQ7KVPWx2whDrQAlsvvIzlsiBe6cO7+5bT3F2y1I6pO2jbs1Ums3f9E1TbwFqY3JccRPhBG9ibZ2RNH6kB1zLZLFVhqGJIrpBrtuONC+OeUMszrmLnZP2Ee04b5xdRUMRxI5jWngemLi6cVdbpNP0jXyvuDOkZvbYxYlj7daLk5SOwGMTfjHJDcfX2xwFdlO2robZ3xJnuowaBmSrlSuzPZwMj1Kx9P542ciQmJTcf2dF3ihnACitdXOMBGk0smx+4gxyvcmgNc95AfF43FkC6Ec3tiIQuevH2tPvEW8UbFl/u7EF8RKQhP0nSlygqEVBVJ7p2UYEp3Mz9GyUMc0xKjC8S4CDVf4HcFkuKL7rvEXp+A8PZMahBM22VBuiiBob0nvzv7Sl4WTYABZi9m/PZl27ni1YjrvOGQXkqfzFNoQPP4sY";

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
