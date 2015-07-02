namespace Fanex.Nap.CMS.WinLimitControl.Models.GlobalSetting
{
    using Fanex.Nap.CMS.Settings.Models;

    public class WinLimitEntity
    {
        public BaseSetting Currency { get; set; }

        public BaseSetting Game { get; set; }

        public decimal MaxWinLimit { get; set; }
    }
}
