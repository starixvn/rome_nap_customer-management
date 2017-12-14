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

        public IEnumerable<GlobalSettingItem> GetList(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<GlobalSettingItem>(criteria.GetSpParams());
            }
        }

        public Result Add(ICriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public Result Edit(ICriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        public Result Delete(ICriteria criteria)
        {
            return ExecuteNonQuery(criteria);
        }

        private static IdNameModel GetCurrency(IDataReader reader)
        {
            return new IdNameModel
            {
                Id = Convert.ToInt32(reader["exchangeid"]),
                Name = reader["currency"].ToString()
            };
        }

        private static Result ExecuteNonQuery(ICriteria criteria)
        {
            Result result = new Result();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();

                result.Code = Convert.ToInt32(outputParams["errcode"]);
                result.Message = outputParams["errmsg"].ToString();
            }

            return result;
        }
    }
}
