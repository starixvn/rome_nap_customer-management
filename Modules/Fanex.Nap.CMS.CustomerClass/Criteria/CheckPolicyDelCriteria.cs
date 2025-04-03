using Fanex.Nap.CMS.CustomerClass.Requests;
using Fanex.Nap.CMS.Infrastructure.Configurations;
using Fanex.Nap.CMS.Infrastructure.Criteria;

namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class CheckPolicyDelCriteria : CommandBaseCriteria
    {
        public bool IsTest { get; } = AppSettings.IsTest;

        public string Policies { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicyCheck";
    }
}
