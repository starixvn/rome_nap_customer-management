namespace Fanex.Nap.CMS.PayoutManagement.Constants
{
    using System.Configuration;

    public static class PayoutManagementConst
    {
        public static readonly string MicroGamingResultAPI;
        public static readonly string ColossusBetsAPIAuthorizationURL;
        public static readonly string ColossusBetsAPITicketsURL;
        public static readonly string WinningCode;

        static PayoutManagementConst()
        {
            var appSettings = ConfigurationManager.AppSettings;

            MicroGamingResultAPI = appSettings["MicroGamingResultAPI"];
            ColossusBetsAPIAuthorizationURL = appSettings["ColossusBetsAPIAuthorizationURL"];
            ColossusBetsAPITicketsURL = appSettings["ColossusBetsAPITicketsURL"];

            WinningCode = "WinningCode";
        }
    }
}
