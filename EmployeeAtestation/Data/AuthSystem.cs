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
        private const string _storedDataEncrypted = "fJcH3OFKpjMRXRc8ICYeWP0BcOU+G9RGsrg0jcI4PchdsHJumN/70fk+oVagzMiKp9V47zEz5YStERNUWuTLFMcjFqpazRoJPr7UHboqyc+/dZZAMRvqLJJfjvfUK9mpnxF+Ja7YX4u51pSfbprnx0STH0xmCqx6JH1JMHkMNuxsLH45Kzgb2MH1KoZ1/rnWV+luIfVeGdb+an4DKbrKV3pkj5C6n/YxyF/A8GA+rlk+dMmx14eDUtlS+sZxyuTFTnJNBaZn22wDk4CAMMub4oTsmAooXFCKfHCBTBHBwKXSTBWfUNCq0uvzbYY2BkyyZaejOo+C6CaWbg1tr4ZrDxxiq6Q6IZlMoOu2fVHpjN5IhSwrc8M5SeQ1EGNwXkAgb8n5RJFIQ+2X/Tl9ztGicAP7JxZFiX1FlOSiRvljGGQq3HfF8dunLkYusRUt5c6a1b8DbjN6fZCLkYbAqwhp5gkAj0eUWnOl8A5Hg2gU3F/TJ/E2YQBOu/IUCfEN9KqawriItBYlZ940391h4+zLsAuBKQvp6mMRhU3ZrlIeM993DogiieASegkja2QsUSvv+2uEO/16mzBkKEeGTDoLRNK82S+Go32JVUpyW5iBIb4zXvcuY2lHTriBQjpR/AUQ78RqzazNT5X1BsdaoHAZXRF5S4JYdwHWxGB5u5Nub9iL9SzQVmGvWhT0HTR9dMIc3EXCvB/ZTiN1YTjNIvnOrQUd5QPMTUd13gAKJ5N7xZ5TlVWf/5IXOBT5TxrkOIW/";

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
