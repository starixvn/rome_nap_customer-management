namespace Fanex.Nap.CMS.ManageProductSetting.Repositories.CompanyBetLimitSetting
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.Framework.Data;
    using Fanex.Data;
    using Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting;

    public interface ICompanyBetLimitRepository
    {
        IEnumerable<IdNameModel> GetCurrencies(ICriteria criteria);
        IEnumerable<IdNameModel> GetProducts(ICriteria criteria);
        IEnumerable<CompanyBetLimitSetting> GetBetLimitSettings(ICriteria criteria);
    }

    public class CompanyBetLimitRepository : ICompanyBetLimitRepository
    {
        public IEnumerable<IdNameModel> GetCurrencies(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return new IdNameModel
                        {
                            Name = reader["Currency"].ToString(),
                            Id = int.Parse(reader["ExchangeID"].ToString())
                        };
                    }
                }
            }
        }

        public IEnumerable<IdNameModel> GetProducts(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return new IdNameModel
                        {
                            Name = reader["ProductName"].ToString(),
                            Id = int.Parse(reader["ProductID"].ToString())
                        };
                    }
                }
            }
        }

        public IEnumerable<CompanyBetLimitSetting> GetBetLimitSettings(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return new CompanyBetLimitSetting
                        {
                            SportType = int.Parse(reader["Sporttype"].ToString()),
                            SportName = reader["Sportname"].ToString(),
                            CurrencyGroupID = int.Parse(reader["CurrencyGroupID"].ToString()),
                            Currency = reader["Currency"].ToString(),
                            GameGroup = reader["GameGroup"].ToString(),
                            MinBet = decimal.Parse(reader["MinBet"].ToString()),
                            MaxBet = decimal.Parse(reader["MaxBet"].ToString()),
                            Remark = reader["Remark"].ToString(),
                        };
                    }
                }
            }
        }
    }
}