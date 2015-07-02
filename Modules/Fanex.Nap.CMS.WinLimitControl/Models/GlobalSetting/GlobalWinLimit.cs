namespace Fanex.Nap.CMS.WinLimitControl.Models.GlobalSetting
{
    using System.Collections.Generic;

    public class GlobalWinLimit
    {
        public string Currency { get; set; }

        public IList<WinLimitEntity> WinLimits { get; set; }
    }
}
