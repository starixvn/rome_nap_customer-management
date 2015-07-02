namespace Fanex.Nap.CMS.WinLimitControl.Repositories.PlayerWinLimitManagement
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Models.PlayerWinLimitManagement;

    public class PlayerSettingRepository : IPlayerSettingRepository
    {
        public IList<PlayerWinLimitEntity> GetPlayerWinLimits(ICriteria criteria)
        {
            var winLimits = new List<PlayerWinLimitEntity>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        winLimits.Add(ToPlayerWinLimitEntity(reader));
                    }
                }
            }

            return winLimits;
        }

        public AddOrUpdatePlayerWinLimit GetUnsetGameByPlayer(ICriteria criteria)
        {
            var gameWinLimits = new List<GameWinLimit>();
            var unsetGames = new AddOrUpdatePlayerWinLimit 
            {
                WinLimits = gameWinLimits
            };

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        unsetGames.Player = new BaseSetting { Id = Converter.ToInt(reader["CustId"]) };
                        unsetGames.Currency = Converter.ToString2(reader["CurrencyName"]);

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                gameWinLimits.Add(ToAddOrEditPlayerWinLimit(reader));
                            }
                        }
                    }

                    var outputParams = db.GetOuputParams();
                    unsetGames.DbResult = new Result(Converter.ToInt(outputParams["errcode"]), Converter.ToString2(outputParams["errmsg"]));
                }
            }

            return unsetGames;
        }

        public Result DeletePlayerWinLimit(ICriteria criteria)
        {
            return ExecuteNonQuery(criteria);
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

        private PlayerWinLimitEntity ToPlayerWinLimitEntity(IDataReader reader)
        {
            return new PlayerWinLimitEntity
            {
                Player = new BaseSetting 
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
                MaxWinLimit = Converter.ToDecimal(reader["MaxWinLimit"])
            };
        }

        private GameWinLimit ToAddOrEditPlayerWinLimit(IDataReader reader)
        {
            return new GameWinLimit
            { 
                Id = Converter.ToInt(reader["ProductId"]),
                GlobalWinLimit = Converter.ToDecimal(reader["CurrencyMaxWinLimit"])
            };
        }
    }
}
