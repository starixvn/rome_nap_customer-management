namespace Fanex.Nap.CMS.WinLimitControl.Models.CurrencySetting
{
    using Fanex.Nap.CMS.Settings.Models;

    public class WinLimitEntity
    {
        public BaseSetting Game { get; set; }

        public BaseSetting Currency { get; set; }

        public decimal MaxWinLimit { get; set; }
    }
}
