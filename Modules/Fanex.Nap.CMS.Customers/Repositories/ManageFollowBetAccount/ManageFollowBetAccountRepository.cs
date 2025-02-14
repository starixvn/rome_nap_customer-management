namespace Fanex.Nap.CMS.Customers.Repositories.ManageFollowBetAccount
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web.Mvc;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Customers.Models.ManageFollowBetAccount;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class ManageFollowBetAccountRepository : IManageFollowBetAccountRepository
    {
        public IEnumerable<SelectListItem> GetSites(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
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
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
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
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
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
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return db.Query<FollowBetAccountItem>(criteria);
            }
        }

        public void ExecuteNonQuery(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria);
            }
        }
    }
}
