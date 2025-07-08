using Fanex.Nap.CMS.Infrastructure.Criteria;

namespace Fanex.Nap.CMS.DomainManagement.Criteria
{
    public class InsertAgentDomainCriteria : CommandBaseCriteria
    {
        public override string GetSettingKey()
            => "NAP_Domain_CashDomainIns";
        
        public string DomainList { get; set; }
    }
} 