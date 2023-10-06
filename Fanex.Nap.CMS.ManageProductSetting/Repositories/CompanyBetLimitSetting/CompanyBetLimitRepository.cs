namespace Fanex.Nap.CMS.ManageProductSetting.Repositories.CompanyBetLimitSetting
{
    using System;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.Framework.Data;
    using Fanex.Data;
    using Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting;

    public interface ICompanyBetLimitRepository
    {
        IEnumerable<IdNameModel> GetCurrencies(ICriteria criteria);
        IEnumerable<IdNameModel> GetProducts(ICriteria criteria);
        CompanyBetLimitSettingQueryResult GetBetLimitSettings(ICriteria criteria);
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

        public CompanyBetLimitSettingQueryResult GetBetLimitSettings(ICriteria criteria)
        {
            var entries = new List<CompanyBetLimitSetting>();
            var totalRows = 0;
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        entries.Add(new CompanyBetLimitSetting
                        {
                            SportType = int.Parse(reader["Sporttype"].ToString()),
                            SportName = reader["Sportname"].ToString(),
                            CurrencyGroupID = int.Parse(reader["CurrencyGroupID"].ToString()),
                            Currency = reader["Currency"].ToString(),
                            GameGroup = reader["GameGroup"].ToString(),
                            MinBet = decimal.Parse(reader["MinBet"].ToString()),
                            MaxBet = decimal.Parse(reader["MaxBet"].ToString()),
                            Remark = reader["Remark"].ToString(),
                        });
                    }
                }

                totalRows = Convert.ToInt32(db.GetOuputParams()["totalrows"]);
            }

            return new CompanyBetLimitSettingQueryResult
            {
                Entries = entries,
                TotalRows = totalRows
            };
        }
    }
}