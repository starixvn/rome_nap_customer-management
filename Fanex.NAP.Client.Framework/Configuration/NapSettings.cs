namespace Fanex.NAP.Client.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Configuration;

    public interface INapSettings
    {
        bool IsNapHub { get; }

        bool EnableOtp { get; }

        bool EnableDirectLogin { get; }

        bool EnableReCheckIn { get; }

        int SiteId { get; }

        string HubUrl { get; }

        string Environment { get; }

        string UnauthorizeUrl { get; }

        string ServiceUrls { get; }

        int CacheDuration { get; }

        bool? ByPassProxyLocal { get; }
    }

    public class NapSettings : INapSettings
    {
        public NapSettings()
        {
            SiteId = GetValue<int>("Fanex.NAP.Client:SiteId", 1);
            EnableOtp = GetValue<bool>("Fanex.NAP.Client:EnableOtp", false);
            EnableDirectLogin = GetValue<bool>("Fanex.NAP.Client:EnableDirectLogin", false);
            EnableReCheckIn = GetValue<bool>("Fanex.NAP.Client:EnableReCheckIn", false);
            HubUrl = GetValue<string>("Fanex.NAP.Client:HubUrl", string.Empty);
            Environment = GetValue<string>("Fanex.NAP.Client:Environment", "UAT");
            UnauthorizeUrl = GetValue<string>("Fanex.NAP.Client:UnauthorizeUrl", "~/Auth");
            ServiceUrls = GetValue<string>("Fanex.NAP.Client:ServiceUrls", string.Empty);
            AccessKey = GetValue<string>("Fanex.NAP.Client:AccessKey", string.Empty);
            CacheDuration = GetValue<int>("Fanex.NAP.Client:CacheDuration", 20);
            ByPassProxyLocal = GetValue<bool?>("Fanex.NAP.Client:ByPassProxyLocal", null);
            IsNapHub = SiteId == 0;
        }

        public bool IsNapHub { get; private set; }

        public bool EnableOtp { get; private set; }

        public bool EnableDirectLogin { get; private set; }

        public bool EnableReCheckIn { get; private set; }

        public int SiteId { get; private set; }

        public string HubUrl { get; private set; }

        public string Environment { get; private set; }

        public string UnauthorizeUrl { get; private set; }

        public string ServiceUrls { get; private set; }

        public string AccessKey { get; private set; }

        public int CacheDuration { get; private set; }

        public bool? ByPassProxyLocal { get; private set; }

        private static T GetValue<T>(string key, T defaultValue)
        {
            var value = ConfigurationManager.AppSettings[key];

            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));

            return (T)typeConverter.ConvertFromString(value);
        }
    }
}