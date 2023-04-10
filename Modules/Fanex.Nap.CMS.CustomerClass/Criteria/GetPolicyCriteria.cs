namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class GetPolicyCriteria : QueryCriteriaBase
    {
        public GetPolicyCriteria(int policyId)
            => PolicyId = policyId;

        public int PolicyId { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicySel";
    }
}
