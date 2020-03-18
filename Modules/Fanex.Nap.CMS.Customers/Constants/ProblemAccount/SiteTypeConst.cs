namespace Fanex.Nap.CMS.Customers.Constants.ProblemAccount
{
    public static class SiteTypeConst
    {
        public const byte CN88 = 0;
        public const byte Credit = 1;
        public const byte Licensee = 2;

        public static bool IsCN88(int type) => type == CN88;

        public static bool IsCredit(int type) => type == Credit;
    }
}
