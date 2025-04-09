namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class DeletePolicyCriteria : CustomerClassCommandBaseCriteria
    {
        public string Policies { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicyDel";
    }
}
