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

        private static IdNameModel ToReason(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["reasonid"]),
                Name = reader["reasonname"].ToString()
            };

        public Tuple<IEnumerable<ProblemAccountItem>, IEnumerable<ProblemAccountReason>, int> GetProblemAccounts(ICriteria criteria)
        {
            var isCreditSites = criteria is SearchCreditSitesCriteria;
            IList<ProblemAccountItem> problemAccounts = new List<ProblemAccountItem>();
            IList<ProblemAccountReason> reasons = new List<ProblemAccountReason>();
            int totalItems = 0;

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        problemAccounts.Add(ToProblemAccountItem(reader, isCreditSites));
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            reasons.Add(ToProblemAccountReason(reader));
                        }
                    }
                }

                totalItems = Convert.ToInt32(db.GetOuputParams()["totalitems"]);
            }

            return new Tuple<IEnumerable<ProblemAccountItem>, IEnumerable<ProblemAccountReason>, int>(problemAccounts, reasons, totalItems);
        }

        private static ProblemAccountItem ToProblemAccountItem(IDataReader reader, bool isCreditSites)
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
                isCreditSites ? null : reader["informed1by"].ToString(),
                isCreditSites ? null : reader["informed2by"].ToString(),
                isCreditSites ? reader["currency"].ToString() : null );

        private static decimal? ToNullableDecimal(object value)
            => value == null || value == DBNull.Value ? default(decimal?) : Convert.ToDecimal(value);

        private static int? ToNullableInt32(object value)
            => value == null || value == DBNull.Value ? default(int?) : Convert.ToInt32(value);

        private static ProblemAccountReason ToProblemAccountReason(IDataReader reader)
            => new ProblemAccountReason(
                Convert.ToInt32(reader["custid"]),
                reader["username"].ToString(),
                Convert.ToDateTime(reader["createdate"]),
                reader["reasonname"].ToString());

        public Result MarkNew(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var ouputParams = db.GetOuputParams();

                return new MarkNewResult(
                    Convert.ToInt32(ouputParams["errcode"]),
                    ouputParams["errmsg"].ToString(),
                    ouputParams["successlist"].ToString(),
                    ouputParams["faillist"].ToString());
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

        public IEnumerable<IdNameModel> GetCurrencies(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return ToCurrency(reader);
                    }
                }
            }
        }

        private static IdNameModel ToCurrency(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["exchangeid"]),
                Name = reader["currency"].ToString()
            };
    }
}
