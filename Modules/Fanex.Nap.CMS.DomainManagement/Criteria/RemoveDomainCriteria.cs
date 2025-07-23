using Fanex.Nap.CMS.Infrastructure.Configurations;
using Fanex.Nap.CMS.Infrastructure.Criteria;

namespace Fanex.Nap.CMS.DomainManagement.Criteria
{
    public class RemoveDomainCriteria : CommandBaseCriteria
    {
        public string DomainList { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public string CashSite { get; } = AppSettings.CashOfferedSites;

        public override string GetSettingKey()
            => "NAP_Domain_Del";

        public override bool IsValid()
            => true;
    }
}
