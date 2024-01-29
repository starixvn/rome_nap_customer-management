namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class GetRuleListCriteria : QueryCriteriaBase
    {
        public GetRuleListCriteria(SearchRuleRequest request, string leagueGroups)
        {
            CustomerClass = request.CustomerClass;
            SportType = request.SportType;
            LeagueGroup = request.LeagueGroups ?? string.Empty;
            IsLive = request.IsLive;
            LeagueGroups = leagueGroups;
        }

        public short CustomerClass { get; }

        public short SportType { get; }

        public string LeagueGroup { get; }

        public string LeagueGroups { get; }

        public byte IsLive { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassRuleSearch";
    }
}
