namespace Fanex.Nap.CMS.WinLimitControl.Models.PlayerSetting
{
    using Fanex.Nap.CMS.Settings.Models;

    public class UndefinedPlayerWinLimit
    {
        public BaseSetting Player { get; set; }

        public string Currency { get; set; }

        public BaseSetting Game { get; set; }

        public decimal PlayerWinLimit { get; set; }

        public decimal GlobalWinLimit { get; set; }
    }
}
