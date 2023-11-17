using Fanex.Nap.CMS.CustomerClass.Requests;

namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class CheckRuleDelCriteria : QueryCriteriaBase
    {
        public CheckRuleDelCriteria(DeleteRuleRequest request)
        {
            RuleIDs = request.RuleIDs;
        }
        public string RuleIDs { get; }

        public int ErrCode { get; }

        public string ErrMsg { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassRuleCheck";
    }
}
