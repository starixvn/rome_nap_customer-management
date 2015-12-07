namespace Fanex.Nap.CMS.PayoutManagement.Constants
{
    using System.Configuration;

    public static class PayoutManagementConst
    {
        public static readonly string MicroGamingResultAPI;
        public static readonly string ColossusBetsAPIAuthorizationURL;
        public static readonly string ColossusBetsAPITicketsURL;

        static PayoutManagementConst()
        {
            var appSettings = ConfigurationManager.AppSettings;

            MicroGamingResultAPI = appSettings["MicroGamingResultAPI"];
            ColossusBetsAPIAuthorizationURL = appSettings["ColossusBetsAPIAuthorizationURL"];
            ColossusBetsAPITicketsURL = appSettings["ColossusBetsAPITicketsURL"];
        }
    }
}
