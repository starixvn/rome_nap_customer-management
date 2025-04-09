using Fanex.Nap.CMS.Infrastructure.Configurations;
using Fanex.Nap.CMS.Infrastructure.Criteria;

namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class CheckRuleDelCriteria : CommandBaseCriteria
    {
        public bool IsTest { get; } = AppSettings.IsTest;

        public string RuleIDs { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassRuleCheck";
    }
}
