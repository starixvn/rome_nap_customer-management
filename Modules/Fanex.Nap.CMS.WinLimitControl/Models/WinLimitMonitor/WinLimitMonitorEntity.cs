namespace Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor
{
    using Fanex.Nap.CMS.Settings.Models;

    public class WinLimitMonitorEntity
    {
        public BaseSetting PlayerInfo { get; set; }

        public BaseSetting Currency { get; set; }

        public BaseSetting Game { get; set; }

        public decimal MaxWinLimit { get; set; }

        public decimal WinningAmount { get; set; }

        public string BlockedTimeDisplay { get; set; }

        public bool IsBlocked { get; set; }
    }
}
