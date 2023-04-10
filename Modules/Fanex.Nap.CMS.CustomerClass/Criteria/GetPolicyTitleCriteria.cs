namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class GetPolicyTitleCriteria : QueryCriteriaBase
    {
        public GetPolicyTitleCriteria(short sportType)
            => SportType = sportType;

        public short SportType { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicyGet";
    }
}
