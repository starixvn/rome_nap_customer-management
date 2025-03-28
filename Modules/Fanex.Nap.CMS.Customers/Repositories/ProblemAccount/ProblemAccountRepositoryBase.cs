namespace Fanex.Nap.CMS.Customers.Repositories.ProblemAccount
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Customers.Criteria.ProblemAccount;
    using Fanex.Nap.CMS.Customers.Criteria.ProblemAccountLicenseeSite;
    using Fanex.Nap.CMS.Customers.Resources;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Infrastructure.Models;
    using Models.ProblemAccount;

    public abstract class ProblemAccountRepositoryBase : IProblemAccountRepository
    {
        private const int ErrorInvalidAccount = 100;
        private const int ErrorExistUnmarkedAccount = 101;
        private const int ErrorNotInSelectedLevelAccount = 102;

        public IEnumerable<IdNameModel> GetCurrencies(CriteriaBase criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
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

        public IEnumerable<IdNameModel> GetReasons(CriteriaBase criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
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

        public IEnumerable<IdNameModel> GetPreventiveActions(CriteriaBase criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
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

        public Tuple<IEnumerable<ProblemAccountItem>, IEnumerable<ProblemAccountReason>, int> GetProblemAccounts(CriteriaBase criteria)
        {
            IList<ProblemAccountItem> problemAccounts = new List<ProblemAccountItem>();
            IList<ProblemAccountReason> reasons = new List<ProblemAccountReason>();
            int totalItems = 0;

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey(), 60 * 5))
            {
                using (var reader = db.ExecuteReader(criteria))
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

                totalItems = Convert.ToInt32(db.GetOutputParams()["totalitems"]);
            }

            return new Tuple<IEnumerable<ProblemAccountItem>, IEnumerable<ProblemAccountReason>, int>(problemAccounts, reasons, totalItems);
        }

        protected abstract ProblemAccountItem ToProblemAccountItem(IDataReader reader);

        protected static decimal? ToNullableDecimal(object value)
            => value == null || value == DBNull.Value ? default(decimal?) : Convert.ToDecimal(value);

        protected static double? ToNullableDouble(object value)
            => value == null || value == DBNull.Value ? default(double?) : Convert.ToDouble(value);

        protected static int? ToNullableInt32(object value)
            => value == null || value == DBNull.Value ? default(int?) : Convert.ToInt32(value);

        protected virtual ProblemAccountReason ToProblemAccountReason(IDataReader reader)
            => new ProblemAccountReason(
                Convert.ToInt32(reader["custid"]),
                reader["username"].ToString(),
                Convert.ToDateTime(reader["createdate"]),
                reader["reasonname"].ToString());

        public Result MarkNew(MarkNewCriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria);

                return new MarkNewResult(criteria.ErrCode, criteria.ErrMsg, criteria.SuccessList, criteria.FailList);
            }
        }

        public Tuple<Result, string> Unmark(UnmarkLicenseeSiteCriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader.Read())
                    {
                        var error = MapToUnMarkMessage(
                            criteria.ErrCode,
                            reader["username"].ToString());

                        return new Tuple<Result, string>(error, string.Empty);
                    }
                }

                var result = new Result(criteria.ErrCode, criteria.ErrMsg);
                var custIds = criteria.SuccessAllDownlineList;

                return new Tuple<Result, string>(result, custIds);
            }
        }

        private static Result MapToUnMarkMessage(int errorCode, string username)
        {
            string errorMessage;
            var newUsername = string.Join(", ", username.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            if (errorCode == ErrorInvalidAccount)
            {
                errorMessage = string.Format(ProblemAccountResource.ErrorInvalidAccount, newUsername);
            }
            else if (errorCode == ErrorExistUnmarkedAccount)
            {
                errorMessage = string.Format(ProblemAccountResource.ErrorExistingUnmarked, newUsername);
            }
            else if (errorCode == ErrorNotInSelectedLevelAccount)
            {
                errorMessage = string.Format(ProblemAccountResource.ErrorNotInSelectedLevelAccount, newUsername);
            }
            else
            {
                errorMessage = $"{errorCode}: {newUsername}";
            }

            return new Result(errorCode, errorMessage);
        }

        public Tuple<Result, IEnumerable<CTSCustomerReason>> Update(NonQueryWithoutRoleIdCriteriaBase criteria, byte newReasonId)
        {
            var ctsCustomerReasons = new List<CTSCustomerReason>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        ctsCustomerReasons.Add(new CTSCustomerReason(
                            Convert.ToInt32(reader["custid"]),
                            ToInt32(reader["oldreasonid"]),
                            newReasonId));
                    }
                }

                return new Tuple<Result, IEnumerable<CTSCustomerReason>>(new Result(criteria.ErrCode, criteria.ErrMsg), ctsCustomerReasons);
            }
        }

        public Result ExecuteNonQuery(CommandBaseCriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria);

                return new Result(criteria.ErrCode, criteria.ErrMsg);
            }
        }

        private static int? ToInt32(object value)
            => value == null || value == DBNull.Value
            ? default(int?)
            : Convert.ToInt32(value);
    }
}
