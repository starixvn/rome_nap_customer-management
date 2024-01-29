namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class GetRuleListBySportType : QueryCriteriaBase
    {
        public GetRuleListBySportType(short sportType, string leagueGroups)
        {
            SportType = sportType;
            LeagueGroups = leagueGroups;
        }

        public short SportType { get; }
        public string LeagueGroups { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassSportGet";
    }
}
