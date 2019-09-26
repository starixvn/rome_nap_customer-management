namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    public class SearchAccountCriteria : SearchBaseCriteria
    {
        public string UserName { get; set; }

        public int RoleId { get; set; }

        public string Status { get; set; }

        public int SiteId { get; set; }

        public string CurrencyIds { get; set; }

        public SearchAccountCriteria() : this(0, 0) { }

        public SearchAccountCriteria(int pageSize, int pageIndex) : base(pageSize, pageIndex) { }

        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_CustomerSel";
        }

        public override bool IsValid()
        {
            return RoleId >= 0 && RoleId <= 4;
        }
    }
}
