namespace Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Resources;

    public class UnblockedWinLimitModel
    {
        public BaseSetting Player { get; set; }

        public BaseSetting Currency { get; set; }

        public BaseSetting Game { get; set; }

        public decimal WinLimit { get; set; }

        public decimal TodayWinningAmount { get; set; }

        public DateTime LastBlockedTime { get; set; }

        public DateTime LastUnblockedTime { get; set; }

        public string UnblockedByUsername { get; set; }

        public string LastBlockedTimeDisplay
        {
            get
            {
                return LastBlockedTime.ToLongFormatedDateWithSlash();
            }
        }

        public string LastUnblockedTimeDisplay
        {
            get
            {
                return LastUnblockedTime.ToLongFormatedDateWithSlash();
            }
        }

        public string UnblockedByUsernameDisplay
        {
            get
            {
                return string.IsNullOrEmpty(UnblockedByUsername) ? WinLimitControlLabels.System : UnblockedByUsername;
            }
        }
    }
}
