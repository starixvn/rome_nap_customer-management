namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class EditRuleCriteria : AddRuleCriteria
    {
        public EditRuleCriteria(AddRuleRequest request, int ruleId) : base(request, ruleId)
        {
        }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassRuleUpd";
    }
}
