using Fanex.Nap.CMS.CustomerClass.Requests;

namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class CheckPolicyDelCriteria : QueryCriteriaBase
    {
        public CheckPolicyDelCriteria(DeletePolicyRequest request)
        {
            Policies = request.Policies;
        }
        public string Policies { get; }

        public int ErrCode { get; }

        public string ErrMsg { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicyCheck";
    }
}
