namespace Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor
{
    using System;
    using Fanex.Nap.CMS.Settings.Models;

    public class GameWinLimitMonitorModel
    {
        public BaseSetting Game { get; set; }

        public decimal MaxWinLimit { get; set; }

        public decimal WinningAmount { get; set; }

        public decimal PercentageUsed { get; set; }

        public string BlockedTimeDisplay { get; set; }

        public bool IsBlocked { get; set; }

        public static GameWinLimitMonitorModel FromWinLimitMonitorEntity(WinLimitMonitorEntity entity)
        {
            return new GameWinLimitMonitorModel
            {
                Game = entity.Game,
                MaxWinLimit = entity.MaxWinLimit,
                WinningAmount = entity.WinningAmount,
                PercentageUsed = CalculatePercentageUsed(entity),
                BlockedTimeDisplay = entity.BlockedTimeDisplay,
                IsBlocked = entity.IsBlocked
            };
        }

        public static decimal CalculatePercentageUsed(WinLimitMonitorEntity entity)
        {
            return entity.MaxWinLimit == 0 ? 100 : (entity.WinningAmount / entity.MaxWinLimit * 100);
        }
    }
}
