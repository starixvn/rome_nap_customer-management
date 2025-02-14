namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class AddPolicyCriteria : CustomerClassCommandBaseCriteria
    {
        public AddPolicyCriteria(AddPolicyRequest request)
            : base(request.UserId, request.IP)
        {
            SportType = request.SportType;
            PolicyTitle = request.PolicyTitle;
            RuleIds = request.RuleIds;
        }

        public short SportType { get; }

        public string PolicyTitle { get; set; }

        public string RuleIds { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicyIns";
    }
}
