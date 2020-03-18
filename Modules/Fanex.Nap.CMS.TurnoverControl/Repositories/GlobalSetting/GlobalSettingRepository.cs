using System;
using System.Collections.Generic;
using System.Data;
using Fanex.Data;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Repositories.GlobalSetting
{
    public class GlobalSettingRepository : IGlobalSettingRepository
    {
        public IEnumerable<IdNameModel> GetCurrencies(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
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

        public IEnumerable<GlobalSettingItem> GetList(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
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
                reader["currency"].ToString(),
                Convert.ToInt32(reader["profileid"]),
                reader["profilename"].ToString(),
                Convert.ToDecimal(reader["maxturnover"]));

        public IEnumerable<IdNameModel> GetProfiles(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
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

        public Result ExecuteNonQuery(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();

                return new Result(
                    Convert.ToInt32(outputParams["errcode"]),
                    outputParams["errmsg"].ToString());
            }
        }
    }
}
