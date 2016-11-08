namespace Fanex.Nap.CMS.PayoutManagement.Constants
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public static class PayoutManagementConst
    {
        public static readonly string MicroGamingResultAPI;
        public static readonly string ColossusBetsAPIAuthorizationURL;
        public static readonly string ColossusBetsAPITicketsURL;
        public static readonly string WinningCode;

        private static readonly IDictionary<string, string> ColossusBetsDomains;

        static PayoutManagementConst()
        {
            var appSettings = ConfigurationManager.AppSettings;

            MicroGamingResultAPI = appSettings["MicroGamingResultAPI"];
            ColossusBetsAPIAuthorizationURL = appSettings["ColossusBetsAPIAuthorizationURL"];
            ColossusBetsAPITicketsURL = appSettings["ColossusBetsAPITicketsURL"];

            WinningCode = "WinningCode";

            ColossusBetsDomains = GetColossusBetsDomains();
        }

        public static string GetColossusBetsDomainBySiteId(string siteId)
        {
            string domain;

            if (!ColossusBetsDomains.TryGetValue(siteId, out domain))
            {
                domain = string.Empty;
            }

            return domain;
        }

        private static IDictionary<string, string> GetColossusBetsDomains()
        {
            IDictionary<string, string> domains = new Dictionary<string, string>(8);

            var appSettings = ConfigurationManager.AppSettings;
            var keyDomains = appSettings.AllKeys.Where(k => k.Contains("ColossusBets_"));

            foreach (string key in keyDomains)
            {
                domains[key.Replace("ColossusBets_", string.Empty)] = appSettings[key];
            }

            return domains;
        }
    }
}
