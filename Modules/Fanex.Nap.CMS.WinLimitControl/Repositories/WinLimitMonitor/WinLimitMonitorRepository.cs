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

        public Result UnblockAccounts(UnblockAccountsCriteria criteria)
        {
            var updateResult = new Result();
            foreach (var player in criteria.WinLimitMonitor)
            {
                updateResult = ExecuteNonQuery(new UnblockSingleAccountCriteria
                {
                    WinLimitMonitor = player
                });

                if (updateResult.Code != 0)
                {
                    break;
                }
            }

            return updateResult;
        }

        private WinLimitMonitorEntity ToWinLimitMonitorEntity(IDataReader reader)
        {
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
                IsBlocked = Converter.ToBool(reader["IsBlocked"])
            };
        }

        private Result ExecuteNonQuery(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();
                return new Result((int)outputParams["errcode"], outputParams["errmsg"].ToString());
            }
        }
    }
}
