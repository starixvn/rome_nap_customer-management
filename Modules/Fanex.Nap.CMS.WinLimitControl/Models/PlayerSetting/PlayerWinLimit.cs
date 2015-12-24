namespace Fanex.Nap.CMS.WinLimitControl.Models.PlayerSetting
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Models.CurrencySetting;

    public class PlayerWinLimit
    {
        public BaseSetting Player { get; set; }

        public string Currency { get; set; }

        public IList<WinLimitEntity> WinLimits { get; set; }
    }
}
