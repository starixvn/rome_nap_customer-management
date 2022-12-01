namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.NAP.Framework.Data;

    internal class GetRuleByIdCriteria : QueryCriteriaBase
    {
        public GetRuleByIdCriteria(int ruleId)
            => RuleId = ruleId;

        public int RuleId  { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassRuleSel";

        public override bool IsValid()
            => RuleId > 0;
    }
}
