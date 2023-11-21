namespace Fanex.Nap.CMS.CustomerClass.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.NAP.Framework.Data;

    internal interface IPolicyDataAccess : IBaseRepository
    {
        IEnumerable<Policy> GetPolicies(ICriteria criteria);

        IEnumerable<IdNameModel> GetPolicyTitles(ICriteria criteria);

        CheckPolicyResponse CheckPolicy(ICriteria criteria);
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

        public CheckPolicyResponse CheckPolicy(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var affectedPolicies = new List<string>();
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        affectedPolicies.Add(string.Format("Policy {0}", reader["PolicyTitle"].ToString()));
                    }
                }
                var outputParams = db.GetOuputParams();

                var code = Converter.ToInt(outputParams["errcode"]);
                var message = Converter.ToString2(outputParams["errmsg"]);

                return new CheckPolicyResponse(string.Join(", ", affectedPolicies), code, message);
            }
        }
    }
}
