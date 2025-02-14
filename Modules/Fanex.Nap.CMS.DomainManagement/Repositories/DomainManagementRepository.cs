namespace Fanex.Nap.CMS.DomainManagement.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.DomainManagement.Criteria;
    using Fanex.Nap.CMS.DomainManagement.Models;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public interface IDomainManagementRepository
    {
        IEnumerable<Domain> GetDomains(CriteriaBase criteria);

        Domain GetDomain(CriteriaBase criteria);

        void AddOrEditDomain(AddOrEditDomainCriteria criteria);

        Result ExecuteNonQuery(CommandBaseCriteria criteria);

        void AddNormalAnnouncement(CriteriaBase criteria);
    }

    public class DomainManagementRepository : IDomainManagementRepository
    {
        public IEnumerable<Domain> GetDomains(CriteriaBase criteria)
            => Fetch(criteria, BuildDomain);

        public Domain GetDomain(CriteriaBase criteria)
            => Fetch(criteria, BuildDomain).FirstOrDefault();

        private static Domain BuildDomain(IDataReader reader)
        {
            var expiredDate = reader["ExpiredDate"];

            return new Domain(
                Convert.ToInt32(reader["DomainId"]),
                reader["Domain"].ToString(),
                reader["System"].ToString(),
                reader["Site"].ToString(),
                reader["BrandName"].ToString(),
                Convert.ToByte(reader["Status"]),
                reader["RestrictedCountries"].ToString(),
                reader["Remark"].ToString(),
                expiredDate == null || expiredDate == DBNull.Value ? default(DateTime?) : Convert.ToDateTime(expiredDate),
                Convert.ToDateTime(reader["LastUpdateTime"]));
        }

        private static IEnumerable<T> Fetch<T>(CriteriaBase criteria, Func<IDataReader, T> parser)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        yield return parser(reader);
                    }
                }
            }
        }

        public void AddOrEditDomain(AddOrEditDomainCriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria);
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

        public void AddNormalAnnouncement(CriteriaBase criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria);
            }
        }
    }
}
