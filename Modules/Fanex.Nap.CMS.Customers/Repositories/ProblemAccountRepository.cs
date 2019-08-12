namespace Fanex.Nap.CMS.Customers.Repositories
{
    using System;
    using System.Collections.Generic;
    using Criteria.ProblemAccount;
    using Data;
    using Data.Repository;
    using Infrastructure.Models;

    public class ProblemAccountRepository : IProblemAccountRepository
    {
        public IEnumerable<IdNameModel> GetReasons(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    yield return new IdNameModel
                    {
                        Id = Convert.ToInt32(reader["reasonid"]),
                        Name = reader["reasonname"].ToString()
                    };
                }
            }
        }

        public void MarkNew(MarkNewCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var ouputParams = db.GetOuputParams();

                criteria.ErrCode = Convert.ToInt32(ouputParams["errcode"]);
                criteria.ErrMsg = ouputParams["errmsg"].ToString();
                criteria.SuccessList = ouputParams["successlist"].ToString();
                criteria.FailList = ouputParams["faillist"].ToString();
            }
        }

        public Result ExecuteNonQuery(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var ouputParams = db.GetOuputParams();

                return new Result(
                    Convert.ToInt32(ouputParams["errcode"]),
                    ouputParams["errmsg"].ToString());
            }
        }
    }
}
