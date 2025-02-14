namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class EnableRuleCriteria : CustomerClassCommandBaseCriteria
    {
        public EnableRuleCriteria(EnableRuleRequest request)
            : base(request.UserId, request.IP)
        {
            RuleId = request.RuleId;
            Enable = request.Enable;
            TopPriority = request.TopPriority;
        }

        public int RuleId { get; }

        public bool Enable { get; }
        public bool TopPriority { get; }

        public override string GetSettingKey()
            => "NAP_CM_CustomerClassRuleEnable";
    }
}
