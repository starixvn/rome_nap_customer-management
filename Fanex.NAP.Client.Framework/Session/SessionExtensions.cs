namespace Fanex.NAP.Client.Session
{
    using System;
    using System.Web;
    using System.Web.SessionState;
    using Newtonsoft.Json;

    public static class SessionExtensions
    {
        public static void Set<T>(this HttpSessionState session, string key, T value)
        {
            session[key] = JsonConvert.SerializeObject(value);
        }

        public static T Get<T>(this HttpSessionState session, string key)
        {
            var value = session[key];
            return value == null ? default : JsonConvert.DeserializeObject<T>(value.ToString());
        }
    }
}