namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class GetPolicyCriteria : QueryCriteriaBase
    {
        public GetPolicyCriteria(int policyId, string policyTitle)
        {
            PolicyId = policyId;
            PolicyTitle = policyTitle;
        }

        public int PolicyId { get; }
        public string PolicyTitle { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicySel";
    }
}
