namespace Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine
{
    using Fanex.Nap.CMS.Customers.ViewModels.VirtualRateEngine;

    public class UpdateVREngineRuleCriteria : AddNewVREngineRuleCriteria
    {
        public UpdateVREngineRuleCriteria(int ruleId, AddNewRuleViewModel vm, string jsonCheckData)
            : base(vm, jsonCheckData)
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
