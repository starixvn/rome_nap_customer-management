namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class EditPolicyCriteria : AddPolicyCriteria
    {
        public EditPolicyCriteria(int policyId, AddPolicyRequest request) : base(request)
            => PolicyId = policyId;

        public int PolicyId { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicyUpd";
    }
}
