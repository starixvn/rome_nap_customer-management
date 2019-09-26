namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class SearchBaseCriteria : PagingCriteria
    {
        public SearchBaseCriteria() : this(0, 0) { }

        public SearchBaseCriteria(int pageSize, int pageIndex) : base(pageSize, pageIndex) { }

        public bool IsTest => AppSettings.IsTest;
    }
}
