namespace Fanex.Nap.CMS.WinLimitControl.Repositories.PlayerSetting
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerSetting;
    using Fanex.Nap.CMS.WinLimitControl.Models.PlayerSetting;

    public class PlayerSettingRepository : IPlayerSettingRepository
    {
        public IList<dynamic> CustomerAutoSuggestion(ICriteria criteria)
        {
            IList<dynamic> customers = new List<dynamic>(12);

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        customers.Add(new  
                        {
                            label = Converter.ToString2(reader["username"])
                        });
                    }
                }
            }

            return customers;
        }

        public IList<PlayerWinLimitEntity> GetPlayerSettings(GetPlayerWinLimitCriteria criteria)
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

                var outputHash = db.GetOuputParams();
                criteria.TotalRow = (int)outputHash["totalrow"];
            }

            return winLimits;
        }

        public IList<UndefinedPlayerWinLimit> GetUndefinedGameByPlayer(GetUndefinedGameByPlayerCriteria criteria)
        {
            IList<UndefinedPlayerWinLimit> undefinedWinLimits = new List<UndefinedPlayerWinLimit>();
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        undefinedWinLimits.Add(ToUndefinedPlayerWinLimit(reader));
                    }
                }

                var outputParams = db.GetOuputParams();
                criteria.ErrCode = (int)outputParams["errcode"];
                criteria.ErrMsg = (string)outputParams["errmsg"];
            }

            return undefinedWinLimits;
        }

        public Result AddPlayerWinLimit(AddPlayerWinLimitCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public Result UpdatePlayerWinLimit(UpdatePlayerWinLimitCriteria criteria)
        {
            return ExecuteNonQuery(criteria);
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

        private Result ExecuteNonQuery(AddPlayerWinLimitCriteria criteria)
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
                PlayerWinLimit = Converter.ToDecimal(reader["PlayerWinLimit"], -1),
                MaxWinLimit = Converter.ToDecimal(reader["CurrencyWinLimit"], -1)
            };
        }

        private UndefinedPlayerWinLimit ToUndefinedPlayerWinLimit(IDataReader reader)
        {
            return new UndefinedPlayerWinLimit
            {
                Player = new BaseSetting
                {
                    Id = Converter.ToInt(reader["custid"]),
                    Name = Converter.ToString2(reader["Username"])
                },
                Currency = Converter.ToString2(reader["Currency"]),
                Game = new BaseSetting
                {
                    Id = Converter.ToInt(reader["ProductId"]),
                    Name = Converter.ToString2(reader["ProductName"])
                },
                GlobalWinLimit = Converter.ToDecimal(reader["CurrencyWinLimit"], -1)
            };
        }
    }
}
