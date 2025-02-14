using Fanex.Nap.CMS.CustomerClass.Requests;
using Fanex.Nap.CMS.Infrastructure.Configurations;
using Fanex.Nap.CMS.Infrastructure.Criteria;

namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class CheckPolicyDelCriteria : CommandBaseCriteria
    {
        public CheckPolicyDelCriteria(DeletePolicyRequest request)
        {
            Policies = request.Policies;
        }
        public bool IsTest { get; } = AppSettings.IsTest;

        public string Policies { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicyCheck";
    }
}
