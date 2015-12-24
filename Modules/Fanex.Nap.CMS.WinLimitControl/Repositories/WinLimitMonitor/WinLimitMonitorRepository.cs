namespace Fanex.Nap.CMS.WinLimitControl.Repositories.WinLimitMonitor
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor;
    using Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor;

    public class WinLimitMonitorRepository : IWinLimitMonitorRepository
    {
        public IList<WinLimitModel> GetWinLimits(GetWinLimitMonitorCriteria criteria)
        {
            IList<WinLimitModel> winlimits = new List<WinLimitModel>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        winlimits.Add(ToWinLimitModel(reader));
                    }
                }

                var output = db.GetOuputParams();
                criteria.TotalRow = Converter.ToInt(output["totalrow"]);
            }

            return winlimits;
        }

        public Result BlockUnblockPlayers(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());

                var outputParams = db.GetOuputParams();
                return new Result((int)outputParams["errcode"], outputParams["errmsg"].ToString());
            }
        }

        public IList<PlayerWinLimitHistory> GetWinLimitHistories(ICriteria criteria)
        {
            IList<PlayerWinLimitHistory> entities = new List<PlayerWinLimitHistory>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        entities.Add(ToPlayerWinLimitHistory(reader));
                    }
                }
            }

            return entities;
        }

        private WinLimitModel ToWinLimitModel(IDataReader reader)
        {
            return new WinLimitModel
            {
                Player = new BaseSetting
                {
                    Id = Converter.ToInt(reader["CustId"]),
                    Name = Converter.ToString2(reader["Username"])
                },
                Currency = new BaseSetting
                {
                    Id = Converter.ToInt(reader["ExchangeId"]),
                    Name = Converter.ToString2(reader["Currency"])
                },
                Game = new BaseSetting
                {
                    Id = Converter.ToInt(reader["ProductId"]),
                    Name = Converter.ToString2(reader["ProductName"])
                },
                WinLimit = Converter.ToDecimal(reader["WinLimit"]),
                BlockedWinningAmount = Converter.ToDecimal(reader["BlockedWinningAmount"]),
                TotalWinningAmount = Converter.ToDecimal(reader["TotalWinningAmount"]),
                BlockedTime = Converter.ToDateTime(reader["BlockedTime"]),
                BlockedByUsername = Converter.ToString2(reader["BlockedByUsername"]),
                IsNotified = Converter.ToBool(reader["IsNotified"]),
                IsModified = Converter.ToBool(reader["IsModified"])
            };
        }

        private PlayerWinLimitHistory ToPlayerWinLimitHistory(IDataReader reader)
        {
            bool isBlocked = Converter.ToBool(reader["IsBlocked"]);
            return new PlayerWinLimitHistory
            {
                Date = Converter.ToDateTime(reader["Date"]),
                WinLimit = Converter.ToDecimal(reader["WinLimit"]),
                BlockedWinningAmount = isBlocked ? Converter.ToDecimal(reader["BlockedWinAmt"]) : (decimal?)null,
                TotalWinningAmount = Converter.ToDecimal(reader["TotalWinAmt"]),
                IsBlocked = isBlocked,
                ChangedBy = Converter.ToString2(reader["ChangedBy"])
            };
        }
    }
}
