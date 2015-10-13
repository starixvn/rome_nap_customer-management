namespace Fanex.Nap.CMS.ColossusBets.Constants
{
    using System.Configuration;

    public static class ColossusBetsConst
    {
        public static readonly string ColossusBetsAPIAuthorizationURL;
        public static readonly string ColossusBetsAPIVendor;
        public static readonly string ColossusBetsAPIVendorKey;
        public static readonly string ColossusBetsAPIVendorProduct;
        public static readonly string ColossusBetsAPITicketsURL;

        static ColossusBetsConst()
        {
            var appSettings = ConfigurationManager.AppSettings;
            
            ColossusBetsAPIAuthorizationURL = appSettings["ColossusBetsAPIAuthorizationURL"];
            ColossusBetsAPIVendor = appSettings["ColossusBetsAPIVendor"];
            ColossusBetsAPIVendorKey = appSettings["ColossusBetsAPIVendorKey"];
            ColossusBetsAPIVendorProduct = appSettings["ColossusBetsAPIVendorProduct"];

            ColossusBetsAPITicketsURL = appSettings["ColossusBetsAPITicketsURL"];
        }
    }
}
