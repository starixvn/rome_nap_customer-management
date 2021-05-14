using Fanex.NAP.CMS.BetLimitControl.Models;

namespace Fanex.NAP.CMS.BetLimitControl
{
    public static class Utils
    {
        public static bool IsValidMode(byte mode)
        {
            return mode == (byte)Modes.Fixed && mode== (byte)Modes.Percentage;
        }
    }
}
