namespace Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Settings.Models;

    public class WinLimitModel
    {
        public BaseSetting Player { get; set; }

        public BaseSetting Currency { get; set; }

        public BaseSetting Game { get; set; }

        public decimal WinLimit { get; set; }

        public decimal TotalWinningAmount { get; set; }

        public DateTime BlockedTime { get; set; }

        public decimal PercentageUsed
        {
            get
            {
                var roundTotalWinningAmount = Math.Round(TotalWinningAmount, 2);
                return WinLimit == 0 ? 100 : (roundTotalWinningAmount / WinLimit * 100);
            }
        }

        public string BlockedTimeDisplay
        {
            get
            {
                return BlockedTime.ToLongFormatedDateWithSlash();
            }
        }

        public bool IsNotified { get; set; }

        public bool IsBlocked { get; set; }

        public DateTime WinlostDate { get; set; }

        public string WinlostDateDisplay
        {
            get
            {
                return WinlostDate.ToShortFormatedDateWithSlash();
            }
        }
    }
}
