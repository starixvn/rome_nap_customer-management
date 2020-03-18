namespace Fanex.Nap.CMS.TurnoverControl.Constants
{
    public static class SportTypeConst
    {
        public const short Soccer = 1;

        public const short Others = 99;

        public static bool IsValidSportType(short sportType)
            => sportType == Soccer || sportType == Others;
    }
}
