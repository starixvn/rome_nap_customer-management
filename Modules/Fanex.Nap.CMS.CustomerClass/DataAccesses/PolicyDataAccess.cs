namespace Fanex.Nap.CMS.CustomerClass.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.NAP.Framework.Data;

    internal interface IPolicyDataAccess : IBaseRepository
    {
        IEnumerable<Policy> GetPolicies(ICriteria criteria);

        IEnumerable<IdNameModel> GetPolicyTitles(ICriteria criteria);
    }

    internal class PolicyDataAccess : BaseRepository, IPolicyDataAccess
    {
        public IEnumerable<Policy> GetPolicies(ICriteria criteria)
            => ExecuteReader(criteria, ToPolicy);

        private static Policy ToPolicy(IDataReader reader)
            => new Policy(
                Convert.ToInt32(reader["PolicyId"]),
                reader["PolicyTitle"].ToString(),
                Convert.ToInt16(reader["SportType"]),
                DataReaderToModelHelper.ToPolicyRuleList(reader));

        public IEnumerable<IdNameModel> GetPolicyTitles(ICriteria criteria)
            => ExecuteReader(criteria, ToPolicyTitle);

        private static IdNameModel ToPolicyTitle(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["PolicyId"]),
                Name = reader["PolicyTitle"].ToString()
            };
    }
}
