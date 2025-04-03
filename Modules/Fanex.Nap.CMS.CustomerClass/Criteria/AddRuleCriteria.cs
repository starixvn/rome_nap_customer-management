namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class AddRuleCriteria : CustomerClassCommandBaseCriteria
    {
        public string Rules { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_CustomerClassRuleIns";
    }
}
