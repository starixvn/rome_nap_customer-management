namespace Fanex.Nap.CMS.WinLimitControl.Repositories.WinLimitMonitor
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor;

    public class DummyData
    {
        public static IList<WinLimitMonitorEntity> GetWinLimitMonitor(ICriteria criteria)
        {
            var winLimitMonitor = new List<WinLimitMonitorEntity>();
            winLimitMonitor.Add(new WinLimitMonitorEntity
            {
                PlayerInfo = new BaseSetting
                {
                    Id = 1,
                    Name = "Member1"
                },
                Currency = new BaseSetting
                {
                    Id = 43
                },
                Game = new BaseSetting
                {
                    Id = 5
                },

                MaxWinLimit = 1000,
                WinningAmount = 1050,
                BlockedTimeDisplay = Converter.ToString2("6/8/2015 5:45 AM"),
                IsBlocked = false
            });

            winLimitMonitor.Add(new WinLimitMonitorEntity
            {
                PlayerInfo = new BaseSetting
                {
                    Id = 1,
                    Name = "Member1"
                },
                Currency = new BaseSetting
                {
                    Id = 43
                },
                Game = new BaseSetting
                {
                    Id = 3
                },

                MaxWinLimit = 900,
                WinningAmount = 500,
                BlockedTimeDisplay = Converter.ToString2("6/4/2015 12:00 AM"),
                IsBlocked = true
            });

            winLimitMonitor.Add(new WinLimitMonitorEntity
            {
                PlayerInfo = new BaseSetting
                {
                    Id = 1,
                    Name = "Member1"
                },
                Currency = new BaseSetting
                {
                    Id = 43
                },
                Game = new BaseSetting
                {
                    Id = 8
                },

                MaxWinLimit = 1200,
                WinningAmount = 800,
                BlockedTimeDisplay = Converter.ToString2("6/8/2015 5:45 AM"),
                IsBlocked = true
            });

            winLimitMonitor.Add(new WinLimitMonitorEntity
            {
                PlayerInfo = new BaseSetting
                {
                    Id = 1,
                    Name = "Member2"
                },
                Currency = new BaseSetting
                {
                    Id = 30
                },
                Game = new BaseSetting
                {
                    Id = 5
                },

                MaxWinLimit = 500,
                WinningAmount = 700,
                BlockedTimeDisplay = Converter.ToString2("6/8/2015 5:45 AM"),
                IsBlocked = false
            });

            winLimitMonitor.Add(new WinLimitMonitorEntity
            {
                PlayerInfo = new BaseSetting
                {
                    Id = 1,
                    Name = "Member2"
                },
                Currency = new BaseSetting
                {
                    Id = 30
                },
                Game = new BaseSetting
                {
                    Id = 3
                },

                MaxWinLimit = 1500,
                WinningAmount = 900,
                BlockedTimeDisplay = Converter.ToString2("6/8/2015 5:45 AM"),
                IsBlocked = true
            });

            winLimitMonitor.Add(new WinLimitMonitorEntity
            {
                PlayerInfo = new BaseSetting
                {
                    Id = 1,
                    Name = "Member2"
                },
                Currency = new BaseSetting
                {
                    Id = 30
                },
                Game = new BaseSetting
                {
                    Id = 8
                },

                MaxWinLimit = 1900,
                WinningAmount = 1300,
                BlockedTimeDisplay = Converter.ToString2("6/4/2015 12:00 AM"),
                IsBlocked = true
            });

            winLimitMonitor.Add(new WinLimitMonitorEntity
            {
                PlayerInfo = new BaseSetting
                {
                    Id = 1,
                    Name = "Member3"
                },
                Currency = new BaseSetting
                {
                    Id = 35
                },
                Game = new BaseSetting
                {
                    Id = 5
                },

                MaxWinLimit = 2200,
                WinningAmount = 1200,
                BlockedTimeDisplay = Converter.ToString2("6/8/2015 5:45 AM"),
                IsBlocked = false
            });

            winLimitMonitor.Add(new WinLimitMonitorEntity
            {
                PlayerInfo = new BaseSetting
                {
                    Id = 1,
                    Name = "Member3"
                },
                Currency = new BaseSetting
                {
                    Id = 35
                },
                Game = new BaseSetting
                {
                    Id = 8
                },

                MaxWinLimit = 400,
                WinningAmount = 350,
                BlockedTimeDisplay = Converter.ToString2("6/8/2015 5:45 AM"),
                IsBlocked = true
            });

            return winLimitMonitor;
        }

        public static Result UnblockAccounts(ICriteria criteria)
        {
            criteria.GetSpParams();
            return new Result
            {
                Code = 0,
                Message = "asd sdasd asd"
            };
        }
    }
}
