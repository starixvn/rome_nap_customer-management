namespace Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine
{
    using Fanex.Data.Repository;

    public class GetVREngineRuleCriteria : CriteriaBase
    {
        public int RuleId { get; set; }

        public override string GetSettingKey()
           => "NAP_VREngine_RuleDetail_Sel";

        public override bool IsValid()
            => RuleId > 0;
    }
}
