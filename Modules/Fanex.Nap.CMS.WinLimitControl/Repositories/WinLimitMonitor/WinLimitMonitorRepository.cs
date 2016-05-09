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

        public Result UnblockPlayer(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());

                var outputParams = db.GetOuputParams();
                return new Result((int)outputParams["errcode"], outputParams["errmsg"].ToString());
            }
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
                TotalWinningAmount = Converter.ToDecimal(reader["TotalWinningAmount"]),
                BlockedTime = Converter.ToDateTime(reader["BlockedTime"]),
                IsNotified = Converter.ToBool(reader["IsNotified"]),
                IsBlocked = Converter.ToBool(reader["IsBlocked"]),
                WinlostDate = Converter.ToDateTime(reader["WinlostDate"])
            };
        }

        private PlayerWinLimitHistory ToPlayerWinLimitHistory(IDataReader reader)
        {
            return new PlayerWinLimitHistory
            {
                Date = Converter.ToDateTime(reader["Date"]),
                WinLimit = Converter.ToDecimal(reader["WinLimit"]),
                TotalWinningAmount = Converter.ToDecimal(reader["TotalWinAmt"]),
                Action = Converter.ToString2(reader["Action"]),
                ChangedBy = Converter.ToString2(reader["ChangedBy"])
            };
        }
    }
}
