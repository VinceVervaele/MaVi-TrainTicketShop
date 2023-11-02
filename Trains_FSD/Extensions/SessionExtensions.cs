using Newtonsoft.Json;

namespace Trains_FSD.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object? value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        // generic class
        public static T? GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key); // json object
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
