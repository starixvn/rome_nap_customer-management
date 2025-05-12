using System;
using System.Web;
using System.Web.SessionState;
using Newtonsoft.Json;

namespace Fanex.NAP.Client.Session
{
    public class DefaultSessionProvider : ISessionProvider
    {
        private readonly HttpContext _httpContext;

        public DefaultSessionProvider()
        {
            _httpContext = HttpContext.Current;
        }

        public string GetString(string key)
        {
            return _httpContext.Session[key]?.ToString();
        }

        public void SetString(string key, string value)
        {
            _httpContext.Session[key] = value;
        }

        public int? GetInt32(string key)
        {
            var value = _httpContext.Session[key];
            if (value == null)
            {
                return null;
            }
            return (int)value;
        }

        public void SetInt32(string key, int value)
        {
            _httpContext.Session[key] = value;
        }

        public T Get<T>(string key)
        {
            var value = _httpContext.Session[key];
            if (value == null)
            {
                return default(T);
            }

            if (value is T typedValue)
            {
                return typedValue;
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(value.ToString());
            }
            catch
            {
                return default(T);
            }
        }

        public void Set<T>(string key, T value)
        {
            _httpContext.Session[key] = value;
        }

        public void Clear()
        {
            _httpContext.Session.Clear();
        }
    }

    // Existing NapSession and other code remains unchanged
}
