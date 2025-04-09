namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class EditPolicyCriteria : AddPolicyCriteria
    {
        public int PolicyId { get; set;}

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicyUpd";
    }
}
