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
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
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
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                var limitDownlineList = db.Query<LimitDownlineItem>(criteria);
                var outputParams = db.GetOutputParams();
                criteria.TotalItems = Convert.ToInt32(outputParams["totalitems"]);

                return limitDownlineList;
            }
        }

        public AddNewOrRemoveLimitResult ExecuteNonQuery(AddNewLimitCriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria);

                return new AddNewOrRemoveLimitResult(
                    criteria.ErrCode,
                    criteria.ErrMsg,
                    criteria.CntMember);
            }
        }
    }
}
