namespace Fanex.Nap.CMS.Customers.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Criteria.ProblemAccount;
    using Data;
    using Data.Repository;
    using Infrastructure.Models;
    using Models.ProblemAccount;

    public class ProblemAccountRepository : IProblemAccountRepository
    {
        public IEnumerable<IdNameModel> GetReasons(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return ToReason(reader);
                    }
                }
            }
        }

        public Tuple<IEnumerable<ProblemAccountItem>, IEnumerable<ProblemAccountReason>> GetProblemAccounts(SearchCriteria criteria)
        {
            IList<ProblemAccountItem> problemAccounts = new List<ProblemAccountItem>();
            IList<ProblemAccountReason> reasons = new List<ProblemAccountReason>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        problemAccounts.Add(ToProblemAccountItem(reader));
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            reasons.Add(ToProblemAccountReason(reader));
                        }
                    }
                }

                criteria.TotalItems = Convert.ToInt32(db.GetOuputParams()["totalitems"]);
            }

            return new Tuple<IEnumerable<ProblemAccountItem>, IEnumerable<ProblemAccountReason>>(problemAccounts, reasons);
        }

        public void MarkNew(MarkNewCriteria criteria)
        {
            if (criteria.IsValid())
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
        }

        public Result ExecuteNonQuery(ICriteria criteria)
        {
            if (criteria.IsValid())
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

            return new Result();
        }

        private static IdNameModel ToReason(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["reasonid"]),
                Name = reader["reasonname"].ToString()
            };

        private static ProblemAccountItem ToProblemAccountItem(IDataReader reader)
            => new ProblemAccountItem(
                Convert.ToInt32(reader["custid"]),
                reader["username"].ToString(),
                reader["headline"].ToString(),
                reader["super"].ToString(),
                reader["master"].ToString(),
                reader["agent"].ToString(),
                Convert.ToBoolean(reader["isproblemaccsuper"]),
                Convert.ToBoolean(reader["isproblemaccmaster"]),
                Convert.ToBoolean(reader["isproblemaccagent"]),
                Convert.ToBoolean(reader["isproblemacc"]),
                ToNullableDecimal(reader["supermpm"]),
                ToNullableDecimal(reader["supermaxbet"]),
                ToNullableDecimal(reader["mastermpm"]),
                ToNullableDecimal(reader["mastermaxbet"]),
                ToNullableDecimal(reader["agentmpm"]),
                ToNullableDecimal(reader["agentmaxbet"]),
                ToNullableDecimal(reader["mpm"]),
                ToNullableDecimal(reader["maxbet"]),
                Convert.ToByte(reader["custstatus"]),
                Convert.ToDateTime(reader["markdate"]),
                reader["comment"].ToString(),
                ToNullableInt32(reader["abnormalbetnumber"]),
                reader["informed1by"].ToString(),
                reader["informed2by"].ToString());

        private static ProblemAccountReason ToProblemAccountReason(IDataReader reader)
            => new ProblemAccountReason(
                Convert.ToInt32(reader["custid"]),
                reader["username"].ToString(),
                Convert.ToDateTime(reader["createdate"]),
                reader["reasonname"].ToString());

        private static decimal? ToNullableDecimal(object value)
            => value == null || value == DBNull.Value ? default(decimal?) : Convert.ToDecimal(value);

        private static int? ToNullableInt32(object value)
            => value == null || value == DBNull.Value ? default(int?) : Convert.ToInt32(value);
    }
}
