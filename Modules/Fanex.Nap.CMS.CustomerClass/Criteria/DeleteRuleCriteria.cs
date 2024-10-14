using Fanex.Nap.CMS.CustomerClass.Requests;

namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class DeleteRuleCriteria : CommandCriteriaBase
    {
        public DeleteRuleCriteria(DeleteRuleRequest request) : base(request.UserId, request.IP)
        {
            RuleIDs = request.RuleIDs;
        }

        public string RuleIDs { get; }

        public override string GetSettingKey()
            => "NAP_CM_CustomerClassRuleDel";
    }
}
