namespace Fanex.Nap.CMS.Customers.Constants.VirtualRateEngine
{
    public static class ActiveConst
    {
        public const byte PROD = 0;
        public const byte UAT = 1;
        public const byte Inactive = 2;

        public static string MapToActiveName(byte value)
        {
            switch (value)
            {
                case PROD:
                    return nameof(PROD);

                case UAT:
                    return nameof(UAT);

                case Inactive:
                default:
                    return nameof(Inactive);
            }
        }
    }
}
