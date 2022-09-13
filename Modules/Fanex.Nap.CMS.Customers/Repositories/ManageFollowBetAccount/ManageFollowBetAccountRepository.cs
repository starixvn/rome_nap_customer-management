namespace Fanex.Nap.CMS.Customers.Repositories.ManageFollowBetAccount
{
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Customers.Models.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class ManageFollowBetAccountRepository : IManageFollowBetAccountRepository
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

        public IEnumerable<SelectListItem> GetDummyAgents(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return new SelectListItem
                        {
                            Text = reader["username"].ToString(),
                            Value = reader["custid"].ToString()
                        };
                    }
                }
            }
        }

        public IEnumerable<FollowBetAccountItem> GetFollowBetAccountList(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                return db.Query<FollowBetAccountItem>(criteria.GetSpParams());
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
