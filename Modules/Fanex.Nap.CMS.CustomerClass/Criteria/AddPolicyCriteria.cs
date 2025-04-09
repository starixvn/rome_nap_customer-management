namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class AddPolicyCriteria : CustomerClassCommandBaseCriteria
    {
        public short SportType { get; set; }

        public string PolicyTitle { get; set; }

        public string RuleIds { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicyIns";
    }
}
