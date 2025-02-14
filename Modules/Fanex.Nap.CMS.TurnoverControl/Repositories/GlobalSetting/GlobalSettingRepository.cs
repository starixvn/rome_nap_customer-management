using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Repositories;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;

namespace Fanex.Nap.CMS.TurnoverControl.Repositories.GlobalSetting
{
    public class GlobalSettingRepository : BaseRepository, IGlobalSettingRepository
    {
        public IEnumerable<IdNameModel> GetCurrencies(CriteriaBase criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        yield return GetCurrency(reader);
                    }
                }
            }
        }

        private static IdNameModel GetCurrency(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["exchangeid"]),
                Name = reader["currency"].ToString()
            };

        public IEnumerable<GlobalSettingItem> GetList(CriteriaBase criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        yield return GetGlobalSettingItem(reader);
                    }
                }
            }
        }

        private static GlobalSettingItem GetGlobalSettingItem(IDataReader reader)
            => new GlobalSettingItem(
                Convert.ToInt32(reader["custid"]),
                reader["username"].ToString(),
                reader["level"].ToString(),
                Convert.ToBoolean(reader["IsGlobalLockTOLimit"]),
                reader["currency"].ToString(),
                Convert.ToInt32(reader["profileid"]),
                reader["profilename"].ToString(),
                Convert.ToDecimal(reader["maxturnover"]));

        public IEnumerable<IdNameModel> GetProfiles(CriteriaBase criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        yield return GetProfile(reader);
                    }
                }
            }
        }

        private static IdNameModel GetProfile(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["profileid"]),
                Name = reader["profilename"].ToString()
            };
    }
}
