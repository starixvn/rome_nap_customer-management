namespace Fanex.Nap.CMS.WinLimitControl.Repositories.WinLimitMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor;

    public class WinLimitMonitorRepository : IWinLimitMonitorRepository
    {
        public WinLimitMonitorDataResult GetWinLimitMonitor(ICriteria criteria)
        {
            var winLimitMonitor = new WinLimitMonitorDataResult
            {
                WinLimitMonitorEntities = new List<WinLimitMonitorEntity>()
            };

            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = objectDb.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        winLimitMonitor.WinLimitMonitorEntities.Add(ToWinLimitMonitorEntity(reader));
                    }
                }

                var output = objectDb.GetOuputParams();
                winLimitMonitor.TotalPlayers = Converter.ToInt(output["totalrow"]);
            }

            return winLimitMonitor;
        }

        public Result UnblockAccounts(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());

                var outputParams = db.GetOuputParams();
                return new Result((int)outputParams["errcode"], outputParams["errmsg"].ToString());
            }
        }

        private WinLimitMonitorEntity ToWinLimitMonitorEntity(IDataReader reader)
        {
            var unblockedTime = Converter.ToDateTime(reader["UnblockedTime"], DateTime.MinValue);
            return new WinLimitMonitorEntity
            {
                PlayerInfo = new BaseSetting
                {
                    Id = Converter.ToInt(reader["CustId"]),
                    Name = Converter.ToString2(reader["Username"])
                },
                Currency = new BaseSetting
                {
                    Id = Converter.ToInt(reader["ExchangeId"])
                },
                Game = new BaseSetting
                {
                    Id = Converter.ToInt(reader["ProductId"])
                },

                MaxWinLimit = Converter.ToDecimal(reader["WinLimit"]),
                WinningAmount = Converter.ToDecimal(reader["WinningAmount"]),
                BlockedTimeDisplay = Converter.ToDateTime(reader["BlockedTime"]).ToString("M/d/yyyy h:mm tt"),
                UnblockedTimeDisplay = unblockedTime == DateTime.MinValue ? string.Empty : unblockedTime.ToString("M/d/yyyy h:mm tt"),
                IsBlocked = Converter.ToBool(reader["IsBlocked"])
            };
        }
    }
}
