namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class SearchCurrencyCriteria : SearchBaseCriteria
    {
        public SearchCurrencyCriteria() : this(0, 0) { }

        public SearchCurrencyCriteria(int pageSize, int pageIndex) : base(pageSize, pageIndex) { }

        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_CurrencySel";
        }
    }
}
