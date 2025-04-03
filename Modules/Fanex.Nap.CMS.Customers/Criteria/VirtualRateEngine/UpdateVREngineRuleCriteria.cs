namespace Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine
{
    public class UpdateVREngineRuleCriteria : AddNewVREngineRuleCriteria
    {
        public int RuleId { get; set; }

        public override string GetSettingKey()
            => "NAP_VREngine_Rule_Update";

        public override bool IsValid()
            => base.IsValid() && RuleId > 0;
    }
}
