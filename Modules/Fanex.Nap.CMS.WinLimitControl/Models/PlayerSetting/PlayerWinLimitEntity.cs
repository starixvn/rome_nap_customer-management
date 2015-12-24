namespace Fanex.Nap.CMS.WinLimitControl.Models.PlayerSetting
{
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Models.CurrencySetting;

    public class PlayerWinLimitEntity : WinLimitEntity
    {
        public BaseSetting Player { get; set; }

        public decimal PlayerWinLimit { get; set; }
    }
}
