namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class GetRuleByIdCriteria : QueryCriteriaBase
    {
        public GetRuleByIdCriteria(int ruleId)
            => RuleId = ruleId;

        public int RuleId  { get; }

        public override string GetSettingKey()
            => "NAP_CM_CustomerClassRuleSel";

        public override bool IsValid()
            => RuleId > 0;
    }
}
