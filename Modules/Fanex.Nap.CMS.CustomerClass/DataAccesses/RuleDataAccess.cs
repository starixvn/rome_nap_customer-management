namespace Fanex.Nap.CMS.CustomerClass.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.NAP.Framework.Data;
    using Rule = Models.Rule;

    internal interface IRuleDataAccess : IBaseRepository
    {
        Tuple<int, IEnumerable<Rule>> GetPagedRule(ICriteria criteria);

        IEnumerable<Rule> GetRules(ICriteria criteria);

        Rule GetRule(ICriteria criteria);
    }

    internal class RuleDataAccess : BaseRepository, IRuleDataAccess
    {
        public Tuple<int, IEnumerable<Rule>> GetPagedRule(ICriteria criteria)
            => ExecuteReaderPaging(criteria, DataReaderToModelHelper.ToPagedRule);

        public IEnumerable<Rule> GetRules(ICriteria criteria)
            => ExecuteReader(criteria, DataReaderToModelHelper.ToRuleList);

        public Rule GetRule(ICriteria criteria)
            => GetSingle(criteria, DataReaderToModelHelper.ToRule);
    }
}
