using Fanex.Nap.CMS.Infrastructure.Criteria;

namespace Fanex.Nap.CMS.DomainManagement.Criteria
{
    public class UpdateAgentDomainCriteria : InsertAgentDomainCriteria
    {
        public override string GetSettingKey()
            => "NAP_Domain_CashDomainUpd";
    }
}
