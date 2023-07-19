using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.Customers.Repositories.LimitDailyPTUpdate
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Customers.Criteria.LimitDailyPTUpdate;
    using Fanex.Nap.CMS.Customers.Models.LimitDailyPTUpdate;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class LimitDailyPTUpdateRepository : ILimitDailyPTUpdateRepository
    {
        public IEnumerable<SelectListItem> GetSites(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return new SelectListItem
                        {
                            Text = reader["site"].ToString(),
                            Value = reader["siteid"].ToString()
                        };
                    }
                }
            }
        }

        public IEnumerable<SelectListItem> GetProducts(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return new SelectListItem
                        {
                            Text = reader["ProductName"].ToString(),
                            Value = reader["ProductID"].ToString()
                        };
                    }
                }
            }
        }

        public IEnumerable<SelectListItem> GetCurrencies(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return new SelectListItem
                        {
                            Text = reader["Currency"].ToString(),
                            Value = reader["ExchangeID"].ToString()
                        };
                    }
                }
            }
        }

        public IEnumerable<LimitDailyPTUpdateItem> GetLimitDailyPTUpdateList(GetLimitDailyPTUpdateListCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var limitDailyPTUpdateList = db.Query<LimitDailyPTUpdateItem>(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();
                criteria.TotalItems = Convert.ToInt32(outputParams["totalitems"]);

                return limitDailyPTUpdateList;
            }
        }

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
