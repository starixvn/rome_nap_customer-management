namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class DeletePolicyCriteria : CommandCriteriaBase
    {
        public DeletePolicyCriteria(DeletePolicyRequest request) : base(request.UserId, request.IP)
        {
            Policies = request.Policies;
        }

        public string Policies { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicyDel";
    }
}
