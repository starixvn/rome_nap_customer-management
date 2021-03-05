using System;
using System.Collections.Generic;
using Fanex.Data;
using Fanex.Data.Repository;
using Fanex.Nap.CMS.Customers.Criteria.LimitNewDownline;
using Fanex.Nap.CMS.Customers.Models.LimitNewDownline;

namespace Fanex.Nap.CMS.Customers.Repositories.LimitNewDownline
{
    public class LimitNewDownlineRepository : ILimitNewDownlineRepository
    {
        public IEnumerable<string> GetSites(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return reader["site"].ToString();
                    }
                }
            }
        }

        public IEnumerable<LimitDownlineItem> GetLimitDownlineList(GetLimitDownlineListCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var limitDownlineList = db.Query<LimitDownlineItem>(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();
                criteria.TotalItems = Convert.ToInt32(outputParams["totalitems"]);

                return limitDownlineList;
            }
        }

        public AddNewOrRemoveLimitResult ExecuteNonQuery(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var ouputParams = db.GetOuputParams();

                return new AddNewOrRemoveLimitResult(
                    Convert.ToInt32(ouputParams["errcode"]),
                    ouputParams["errmsg"].ToString(),
                    Convert.ToInt32(ouputParams["cntmember"]));
            }
        }
    }
}
