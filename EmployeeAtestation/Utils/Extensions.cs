using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace EmployeeAtestation.Utils
{
    public static class Extensions
    {
        public static string ToSha256Hash(this string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            byte[] hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash).ToLower();
        }

        public static string ToJson<T>(this T value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented);
        }

        public static T FromJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
