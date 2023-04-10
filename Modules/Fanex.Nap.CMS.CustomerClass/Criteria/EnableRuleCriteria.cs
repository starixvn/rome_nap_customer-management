namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class EnableRuleCriteria : CommandCriteriaBase
    {
        public EnableRuleCriteria(EnableRuleRequest request)
            : base(request.UserId, request.IP)
        {
            RuleId = request.RuleId;
            Enable = request.Enable;
        }

        public int RuleId { get; }

        public bool Enable { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassRuleEnable";
    }
}
