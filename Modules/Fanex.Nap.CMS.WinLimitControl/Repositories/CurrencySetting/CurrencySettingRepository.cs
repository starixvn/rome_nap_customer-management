namespace Fanex.Nap.CMS.WinLimitControl.Repositories.CurrencySetting
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Models.CurrencySetting;

    public class CurrencySettingRepository : ICurrencySettingRepository
    {
        public IList<WinLimitEntity> GetCurrencySettings(ICriteria criteria)
        {
            IList<WinLimitEntity> currencySettings = new List<WinLimitEntity>(64);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        currencySettings.Add(ToWinLimitEntity(reader));
                    }
                }
            }

            return currencySettings;
        }

        public Result AddCurrencySetting(ICriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public Result UpdateCurrencySetting(ICriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public Result UpdateRelatedPlayerSetting(ICriteria criteria)
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

        private Result ExecuteNonQuery(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());

                var outputParams = db.GetOuputParams();
                return new Result((int)outputParams["errcode"], outputParams["errmsg"].ToString());
            }
        }

        private WinLimitEntity ToWinLimitEntity(IDataReader reader)
        {
            return new WinLimitEntity
            {
                Currency = new BaseSetting
                {
                    Id = Converter.ToInt(reader["CurrencyId"]),
                    Name = Converter.ToString2(reader["CurrencyName"])
                },
                MaxWinLimit = Converter.ToDecimal(reader["MaxWinLimit"], -1)
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
