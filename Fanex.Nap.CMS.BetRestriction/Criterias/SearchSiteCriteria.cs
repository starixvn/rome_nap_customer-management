namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class SearchSiteCriteria : SearchBaseCriteria
    {
        public SearchSiteCriteria() : this(0, 0) { }

        public SearchSiteCriteria(int pageSize, int pageIndex) : base(pageSize, pageIndex) { }

        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_SiteSel";
        }
    }
}
