namespace Fanex.Nap.CMS.CustomerClass.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Repositories;

    internal interface IPolicyDataAccess : IBaseRepository
    {
        IEnumerable<Policy> GetPolicies(CriteriaBase criteria);

        IEnumerable<IdNameModel> GetPolicyTitles(CriteriaBase criteria);

        CheckPolicyResponse CheckPolicy(CommandBaseCriteria criteria);
    }

    internal class PolicyDataAccess : BaseRepository, IPolicyDataAccess
    {
        public IEnumerable<Policy> GetPolicies(CriteriaBase criteria)
            => ExecuteReader(criteria, ToPolicy);

        private static Policy ToPolicy(IDataReader reader)
            => new Policy(
                Convert.ToInt32(reader["PolicyId"]),
                reader["PolicyTitle"].ToString(),
                Convert.ToInt16(reader["SportType"]),
                DataReaderToModelHelper.ToPolicyRuleList(reader));

        public IEnumerable<IdNameModel> GetPolicyTitles(CriteriaBase criteria)
            => ExecuteReader(criteria, ToPolicyTitle);

        private static IdNameModel ToPolicyTitle(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["PolicyId"]),
                Name = reader["PolicyTitle"].ToString()
            };

        public CheckPolicyResponse CheckPolicy(CommandBaseCriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                var affectedPolicies = new List<string>();
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        affectedPolicies.Add(reader["PolicyTitle"].ToString());
                    }
                }

                return new CheckPolicyResponse(string.Join(", ", affectedPolicies), criteria.ErrCode, criteria.ErrMsg);
            }
        }
    }
}
