namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class DeleteRuleCriteria : CustomerClassCommandBaseCriteria
    {
        public string RuleIDs { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_CustomerClassRuleDel";
    }
}
