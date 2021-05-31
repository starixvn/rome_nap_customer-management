namespace Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine
{
    using Fanex.Data.Repository;

    public class GetVREngineRuleCriteria : CriteriaBase
    {
        public GetVREngineRuleCriteria(int ruleId)
        {
            RuleId = ruleId;
        }

        public int RuleId { get; }

        public override string GetSettingKey()
           => "NAP_VREngine_RuleDetail_Sel";

        public override bool IsValid()
            => RuleId > 0;
    }
}
