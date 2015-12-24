namespace Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.WinLimitControl.Resources;

    public class PlayerWinLimitHistory
    {
        public DateTime Date { get; set; }

        public decimal WinLimit { get; set; }

        public decimal? BlockedWinningAmount { get; set; }

        public decimal TotalWinningAmount { get; set; }

        public bool IsBlocked { get; set; }

        public decimal PercentageUsed
        {
            get
            {
                return WinLimit == 0 ? 100 : (TotalWinningAmount / WinLimit * 100);
            }
        }

        public string ChangedBy { get; set; }

        public string DateDisplay
        {
            get
            {
                return Date.ToLongFormatedDateWithSlash();
            }
        }

        public string Status 
        {
            get
            {
                return IsBlocked ? WinLimitControlLabels.Blocked : WinLimitControlLabels.Unblocked;
            }
        }

        public string ChangedByDisplay 
        {
            get
            {
                return string.IsNullOrEmpty(ChangedBy) ? WinLimitControlLabels.System : ChangedBy;
            }
        }
    }
}
