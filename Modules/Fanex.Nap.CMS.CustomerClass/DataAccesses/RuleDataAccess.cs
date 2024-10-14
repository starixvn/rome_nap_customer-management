namespace Fanex.Nap.CMS.CustomerClass.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Responses;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.NAP.Framework.Data;
    using Rule = Models.Rule;

    internal interface IRuleDataAccess : IBaseRepository
    {
        Tuple<int, IEnumerable<Rule>, Result> GetPagedRule(ICriteria criteria);

        IEnumerable<Rule> GetRules(ICriteria criteria);

        Rule GetRule(ICriteria criteria);

        CheckRuleResponse CheckRule(ICriteria criteria);

        IEnumerable<RuleDetail> GetRuleDetail(GetRuleDetailCriteria criteria);
    }

    internal class RuleDataAccess : BaseRepository, IRuleDataAccess
    {
        public Tuple<int, IEnumerable<Rule>, Result> GetPagedRule(ICriteria criteria)
            => ExecuteReaderPagingWithError(criteria, DataReaderToModelHelper.ToPagedRule);

        public IEnumerable<Rule> GetRules(ICriteria criteria)
            => ExecuteReader(criteria, DataReaderToModelHelper.ToRuleList);

        public Rule GetRule(ICriteria criteria)
            => GetSingle(criteria, DataReaderToModelHelper.ToRule);

        public CheckRuleResponse CheckRule(ICriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var affectedRules = new List<string>();
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        affectedRules.Add(reader["RuleTitle"].ToString());
                    }
                }
                var outputParams = db.GetOuputParams();

                var code = Converter.ToInt(outputParams["errcode"]);
                var message = Converter.ToString2(outputParams["errmsg"]);

                return new CheckRuleResponse(string.Join(", ", affectedRules), code, message);
            }
        }

        public IEnumerable<RuleDetail> GetRuleDetail(GetRuleDetailCriteria criteria)
            => ExecuteReader(criteria, DataReaderToModelHelper.ToRuleDetail);
    }
}
