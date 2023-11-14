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
    using Fanex.Nap.CMS.Infrastructure.Models;

    public interface IDomainManagementRepository
    {
        IEnumerable<Domain> GetDomains(ICriteria criteria);

        Domain GetDomain(ICriteria criteria);

        void AddOrEditDomain(AddOrEditDomainCriteria criteria);

        Result ExecuteNonQuery(ICriteria criteria);

        void AddNormalAnnouncement(ICriteria criteria);
    }

    public class DomainManagementRepository : IDomainManagementRepository
    {
        public IEnumerable<Domain> GetDomains(ICriteria criteria)
            => Fetch(criteria, BuildDomain);

        public Domain GetDomain(ICriteria criteria)
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

        private static IEnumerable<T> Fetch<T>(ICriteria criteria, Func<IDataReader, T> parser)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
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
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());

                var ouputParams = db.GetOuputParams();
                criteria.ErrCode = Convert.ToInt32(ouputParams["errcode"]);
                criteria.ErrMsg = ouputParams["errmsg"].ToString();
                criteria.DomainListOutput = ouputParams["domainlistoutput"].ToString();
            }
        }

        public Result ExecuteNonQuery(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var ouputParams = db.GetOuputParams();

                return new Result(Convert.ToInt32(ouputParams["errcode"]), ouputParams["errmsg"].ToString());
            }
        }

        public void AddNormalAnnouncement(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
            }
        }
    }
}
