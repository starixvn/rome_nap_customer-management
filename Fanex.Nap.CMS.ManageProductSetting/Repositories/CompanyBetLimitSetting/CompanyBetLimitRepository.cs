namespace Fanex.Nap.CMS.ManageProductSetting.Repositories.CompanyBetLimitSetting
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.Framework.Data;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Data;
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
                        yield return new IdNameModel
                        {
                            Name = reader["ProductName"].ToString(),
                            Id = int.Parse(reader["ProductID"].ToString())
                        };
                    }
                }
            }
        }
    }
}