namespace Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine
{
    using Fanex.Nap.CMS.Customers.ViewModels.VirtualRateEngine;

    internal class UpdateVREngineRuleCriteria : AddNewVREngineRuleCriteria
    {
        public UpdateVREngineRuleCriteria(int ruleId, AddNewRuleViewModel vm)
            : base(vm)
        {
            RuleId = ruleId;
        }

        public int RuleId { get; }

        public override string GetSettingKey()
            => "NAP_VREngine_Rule_Update";

        public override bool IsValid()
            => base.IsValid() && RuleId > 0;
    }
}
