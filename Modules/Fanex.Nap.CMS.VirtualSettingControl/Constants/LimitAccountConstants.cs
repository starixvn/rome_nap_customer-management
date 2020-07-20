namespace Fanex.Nap.CMS.VirtualSettingControl.Constants
{
    public static class LimitAccountConstants
    {
        public const int ESports = 10000;

        public const int Basketball = 8000;

        public static int GetLimitAccount(int sportType)
        {
            if (sportType == SportTypeConstants.ESports)
            {
                return ESports;
            }

            if (sportType == SportTypeConstants.Basketball)
            {
                return Basketball;
            }

            return 0;
        }
    }
}
