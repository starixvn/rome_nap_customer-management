namespace Fanex.Nap.CMS.Customers.Constants
{
    public static class SearchFilterSessionKeyConst
    {
        public const string CN88 = "ProblemAccountSearchFilterCN88SessionKey";

        public const string Credit = "ProblemAccountSearchFilterCreditSessionKey";

        public const string Licensee = "ProblemAccountSearchFilterLicenseeSessionKey";

        public static string GetSessionKey(byte siteType)
        {
            if (SiteTypeConst.IsCN88(siteType))
            {
                return CN88;
            }

            return SiteTypeConst.IsCredit(siteType) ? Credit : Licensee;
        }
    }
}
