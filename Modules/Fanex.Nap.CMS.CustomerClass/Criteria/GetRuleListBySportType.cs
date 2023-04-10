namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class GetRuleListBySportType : QueryCriteriaBase
    {
        public GetRuleListBySportType(short sportType)
            => SportType = sportType;

        public short SportType { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassSportGet";
    }
}
