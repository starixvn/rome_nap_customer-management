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
    using Fanex.Data.Repository;
    using Rule = Models.Rule;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    internal interface IRuleDataAccess : IBaseRepository
    {
        Tuple<int, IEnumerable<Rule>, Result> GetPagedRule(CriteriaBase criteria);

        IEnumerable<Rule> GetRules(CriteriaBase criteria);

        Rule GetRule(CriteriaBase criteria);

        CheckRuleResponse CheckRule(CommandBaseCriteria criteria);

        IEnumerable<RuleDetail> GetRuleDetail(GetRuleDetailCriteria criteria);
    }

    internal class RuleDataAccess : BaseRepository, IRuleDataAccess
    {
        public Tuple<int, IEnumerable<Rule>, Result> GetPagedRule(CriteriaBase criteria)
            => ExecuteReaderPagingWithError(criteria, DataReaderToModelHelper.ToPagedRule);

        public IEnumerable<Rule> GetRules(CriteriaBase criteria)
            => ExecuteReader(criteria, DataReaderToModelHelper.ToRuleList);

        public Rule GetRule(CriteriaBase criteria)
            => GetSingle(criteria, DataReaderToModelHelper.ToRule);

        public CheckRuleResponse CheckRule(CommandBaseCriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                var affectedRules = new List<string>();
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader.Read())
                    {
                        affectedRules.Add(reader["RuleTitle"].ToString());
                    }
                }

                return new CheckRuleResponse(string.Join(", ", affectedRules), criteria.ErrCode, criteria.ErrMsg);
            }
        }

        public IEnumerable<RuleDetail> GetRuleDetail(GetRuleDetailCriteria criteria)
            => ExecuteReader(criteria, DataReaderToModelHelper.ToRuleDetail);
    }
}
