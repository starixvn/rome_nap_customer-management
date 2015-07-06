namespace Fanex.Nap.CMS.WinLimitControl.Repositories.CurrencySetting
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Models.CurrencySetting;

    public class CurrencySettingRepository : ICurrencySettingRepository
    {
        public IList<WinLimitEntity> GetGlobalWinLimits(ICriteria criteria)
        {
            var winLimits = new List<WinLimitEntity>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        winLimits.Add(ToWinLimitModel(reader));
                    }
                }
            }

            return winLimits;
        }

        public IList<BaseSetting> GetExcludedCurrencies(ICriteria criteria)
        {
            return QuerySettings(criteria);
        }

        public IList<BaseSetting> GetExcludedGames(ICriteria criteria)
        {
            return QuerySettings(criteria);
        }

        public IList<BaseSetting> GetUnsetCurrencies(ICriteria criteria)
        {
            return QuerySettings(criteria);
        }

        public IList<BaseSetting> GetUnsetGames(ICriteria criteria)
        {
            return QuerySettings(criteria);
        }

        public Result AddGlobalWinLimit(ICriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public IList<GreaterPlayerWinLimit> GetGreaterPlayerWinLimits(ICriteria criteria)
        {
            var winLimits = new List<GreaterPlayerWinLimit>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        winLimits.Add(ToGreaterPlayerWinLimitModel(reader));
                    }
                }
            }

            return winLimits;    
        }

        public Result UpdateGlobalWinLimit(ICriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        private IList<BaseSetting> QuerySettings(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<BaseSetting>(criteria.GetSpParams()).ToList();
            }
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
        
        private WinLimitEntity ToWinLimitModel(IDataReader reader)
        {
            return new WinLimitEntity
            {
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

        private GreaterPlayerWinLimit ToGreaterPlayerWinLimitModel(IDataReader reader)
        {
            return new GreaterPlayerWinLimit
            {
                Username = Converter.ToString2(reader["Username"]),
                CurrentWinLimit = Converter.ToDecimal(reader["CurrentWinLimit"])
            };
        }
    }
}
