namespace Fanex.Nap.CMS.DomainManagement.Criteria
{
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    public class AddDomainCriteria : CommandBaseCriteria
    {
        public string DomainList { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public string DomainListOutput { get; set; } = string.Empty;

        public string CashSite { get; } = AppSettings.CashOfferedSites;

        public override string GetSettingKey()
            => "NAP_Domain_Ins";

        public override bool IsValid()
            => true;
    }
}
