namespace Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine
{
    public class DeleteVREngineRuleCriteria : VREngineCriteriaBase
    {
        public int RuleId { get; set; }

        public override string GetSettingKey()
           => "NAP_VREngine_Rule_Delete";

        public override bool IsValid()
            => base.IsValid() && RuleId > 0;
    }
}
