namespace Fanex.Nap.CMS.Customers.Constants
{
    public static class SearchFilterSessionKeyConst
    {
        public const string CN88 = "ProblemAccountSearchFilterCN88SessionKey";

        public const string CreditDeposit = "ProblemAccountSearchFilterCreditDepositSessionKey";

        public static string GetSessionKey(byte siteType)
            => GetSessionKey(SiteTypeConst.IsCN88(siteType));

        public static string GetSessionKey(bool isCN88)
            => isCN88 ? CN88 : CreditDeposit;
    }
}
