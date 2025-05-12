namespace Fanex.NAP.Client
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Fanex.NAP.Client.Clients;
    using Fanex.NAP.Client.Configuration;
    using Fanex.NAP.Client.Services;
    using System.Web;

    public static class ServiceCollectionExtensions
    {
        private static readonly Lazy<HttpClient> _httpClient = new Lazy<HttpClient>(() => new HttpClient());
        private static readonly Dictionary<string, string> _configuration = new Dictionary<string, string>();

        public static void ConfigureNapClient(string serviceUrl)
        {
            _httpClient.Value.BaseAddress = new Uri(serviceUrl);
            _configuration["Fanex.NAP.Client:ServiceUrls"] = serviceUrl;
            _configuration["Fanex.NAP.Client:Environment"] = "UAT";
            _configuration["Fanex.NAP.Client:SiteId"] = "1";
            _configuration["Fanex.NAP.Client:UnauthorizeUrl"] = "~/Auth";
            _configuration["Fanex.NAP.Client:CacheDuration"] = "20";
            _configuration["Fanex.NAP.Client:EnableDirectLogin"] = "true";
            _configuration["Fanex.NAP.Client:EnableReCheckIn"] = "false";
            _configuration["Fanex.NAP.Client:HubUrl"] = "";
            _configuration["Fanex.NAP.Client:EnableOtp"] = "true";
        }

        public static INapSession CreateNapSession()
        {
            return new NapSession();
        }

        public static INapSettings CreateNapSettings()
        {
            return new NapSettings(new SimpleConfiguration(_configuration));
        }

        public static IGroupService CreateGroupService()
        {
            return new GroupService(new GroupClient(_httpClient.Value));
        }

        public static IUserService CreateUserService()
        {
            return new UserService(new UserClient(_httpClient.Value));
        }

        public static ISecurityService CreateSecurityService()
        {
            return new SecurityService(
                new LoginClient(_httpClient.Value),
                new LogoutClient(_httpClient.Value),
                new CheckinClient(_httpClient.Value),
                new SSOClient(_httpClient.Value),
                new UserInfoClient(_httpClient.Value));
        }

        public static ISiteService CreateSiteService()
        {
            return new SiteService(new SiteClient(_httpClient.Value));
        }
    }

    public class SimpleConfiguration
    {
        private readonly Dictionary<string, string> _configuration;

        public SimpleConfiguration(Dictionary<string, string> configuration)
        {
            _configuration = configuration;
        }

        public string GetValue(string key)
        {
            return _configuration.TryGetValue(key, out var value) ? value : null;
        }

        public T GetValue<T>(string key)
        {
            var value = GetValue(key);
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default;
            }
        }

        public bool GetBooleanValue(string key)
        {
            var value = GetValue(key);
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            value = value.ToLower();
            return value == "true" || value == "1";
        }
    }
}