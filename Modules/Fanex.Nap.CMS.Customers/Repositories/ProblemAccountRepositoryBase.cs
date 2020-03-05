namespace Fanex.Nap.CMS.Customers.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Infrastructure.Models;
    using Models.ProblemAccount;

    public abstract class ProblemAccountRepositoryBase : IProblemAccountRepository
    {
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

        public IEnumerable<IdNameModel> GetPreventiveActions(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return ToPreventiveAction(reader);
                    }
                }
            }
        }

        private static IdNameModel ToPreventiveAction(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["actionid"]),
                Name = reader["actionname"].ToString()
            };

        public Tuple<IEnumerable<ProblemAccountItem>, IEnumerable<ProblemAccountReason>, int> GetProblemAccounts(ICriteria criteria)
        {
            IList<ProblemAccountItem> problemAccounts = new List<ProblemAccountItem>();
            IList<ProblemAccountReason> reasons = new List<ProblemAccountReason>();
            int totalItems = 0;

            using (var db = new ObjectDb(criteria.GetSettingKey(), 60 * 5))
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

                totalItems = Convert.ToInt32(db.GetOuputParams()["totalitems"]);
            }

            return new Tuple<IEnumerable<ProblemAccountItem>, IEnumerable<ProblemAccountReason>, int>(problemAccounts, reasons, totalItems);
        }

        protected abstract ProblemAccountItem ToProblemAccountItem(IDataReader reader);

        protected static decimal? ToNullableDecimal(object value)
            => value == null || value == DBNull.Value ? default(decimal?) : Convert.ToDecimal(value);

        protected static int? ToNullableInt32(object value)
            => value == null || value == DBNull.Value ? default(int?) : Convert.ToInt32(value);

        protected virtual ProblemAccountReason ToProblemAccountReason(IDataReader reader)
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
    }
}
