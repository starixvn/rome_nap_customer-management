namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class EnableRuleCriteria : CustomerClassCommandBaseCriteria
    {
        public int RuleId { get; set; }
        public bool Enable { get; set; }
        public bool TopPriority { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_CustomerClassRuleEnable";
    }
}
