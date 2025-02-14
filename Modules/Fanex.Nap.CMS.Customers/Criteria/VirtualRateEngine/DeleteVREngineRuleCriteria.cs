namespace Fanex.Nap.CMS.Customers.Criteria.VirtualRateEngine
{
    public class DeleteVREngineRuleCriteria : VREngineCriteriaBase
    {
        public DeleteVREngineRuleCriteria(int ruleId, int userId, string ip)
            : base(userId, ip)
        {
            RuleId = ruleId;
        }

        public int RuleId { get; }

        public override string GetSettingKey()
           => "NAP_VREngine_Rule_Delete";

        public override bool IsValid()
            => base.IsValid() && RuleId > 0;
    }
}
