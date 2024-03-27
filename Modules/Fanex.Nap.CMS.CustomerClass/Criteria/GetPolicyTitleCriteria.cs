namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class GetPolicyTitleCriteria : QueryCriteriaBase
    {
        public GetPolicyTitleCriteria(string sportTypes)
            => SportTypes = sportTypes;

        public string SportTypes { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicyGet";
    }
}
