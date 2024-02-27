using Newtonsoft.Json;

namespace Utils
{
    internal static class Extensions
    {
        public static string ToJson<T>(this T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static T FromJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
