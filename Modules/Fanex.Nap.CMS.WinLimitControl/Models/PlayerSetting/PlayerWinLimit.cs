namespace Fanex.Nap.CMS.WinLimitControl.Models.PlayerSetting
{
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Models.CurrencySetting;

    public class PlayerWinLimit : CurrencyWinLimit
    {
        public BaseSetting Player { get; set; }
    }
}
