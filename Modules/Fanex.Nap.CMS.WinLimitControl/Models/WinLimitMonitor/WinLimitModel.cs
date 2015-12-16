namespace Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Resources;

    public class WinLimitModel
    {
        public BaseSetting Player { get; set; }

        public BaseSetting Currency { get; set; }

        public BaseSetting Game { get; set; }

        public decimal WinLimit { get; set; }

        public decimal BlockedWinningAmount { get; set; }

        public decimal TotalWinningAmount { get; set; }

        public DateTime BlockedTime { get; set; }

        public string BlockedByUsername { get; set; }

        public decimal PercentageUsed
        {
            get
            {
                return WinLimit == 0 ? 100 : (TotalWinningAmount / WinLimit * 100);
            }
        }

        public string BlockedTimeDisplay
        {
            get
            {
                return BlockedTime.ToLongFormatedDateWithSlash();
            }
        }

        public string BlockedByUsernameDisplay
        {
            get
            {
                return string.IsNullOrEmpty(BlockedByUsername) ? WinLimitControlLabels.System : BlockedByUsername;
            }
        }

        public bool IsNotified { get; set; }

        public bool IsModified { get; set; }
    }
}
