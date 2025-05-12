namespace Fanex.NAP.Client.Configuration
{
    using System;
    using System.ComponentModel;

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
        private readonly SimpleConfiguration _configuration;

        public NapSettings(SimpleConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            SiteId = _configuration.GetValue<int>("Fanex.NAP.Client:SiteId");
            EnableDirectLogin = _configuration.GetBooleanValue("Fanex.NAP.Client:EnableDirectLogin");
            EnableReCheckIn = _configuration.GetBooleanValue("Fanex.NAP.Client:EnableReCheckIn");
            HubUrl = _configuration.GetValue<string>("Fanex.NAP.Client:HubUrl");
            Environment = _configuration.GetValue<string>("Fanex.NAP.Client:Environment");
            UnauthorizeUrl = _configuration.GetValue<string>("Fanex.NAP.Client:UnauthorizeUrl");
            ServiceUrls = _configuration.GetValue<string>("Fanex.NAP.Client:ServiceUrls");
            CacheDuration = _configuration.GetValue<int>("Fanex.NAP.Client:CacheDuration");
            HubUrl = _configuration.GetValue<string>("Fanex.NAP.Client:HubUrl");
            IsNapHub = _configuration.GetBooleanValue("Fanex.NAP.Client:IsNapHub");
            EnableOtp = _configuration.GetBooleanValue("Fanex.NAP.Client:EnableOtp");
            ByPassProxyLocal = _configuration.GetValue<bool?>("Fanex.NAP.Client:BypassProxyLocal");
        }

        public bool IsNapHub { get; }

        public bool EnableOtp { get; }

        public bool EnableDirectLogin { get; }

        public bool EnableReCheckIn { get; }

        public int SiteId { get; }

        public string HubUrl { get; }

        public string Environment { get; }

        public string UnauthorizeUrl { get; }

        public string ServiceUrls { get; }

        public int CacheDuration { get; }

        public bool? ByPassProxyLocal { get; }
    }
}