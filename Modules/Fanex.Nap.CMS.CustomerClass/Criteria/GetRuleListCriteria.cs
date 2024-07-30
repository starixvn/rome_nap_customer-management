namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class GetRuleListCriteria : QueryCriteriaBase
    {
        public GetRuleListCriteria(SearchRuleRequest request, string leagueGroups, string sportTypes)
        {
            CustomerClass = request.CustomerClass ?? string.Empty;
            CustomerClassDetail = request.CustomerClassDetail ?? string.Empty;
            SportTypes = sportTypes;
            LeagueGroup = request.LeagueGroups ?? string.Empty;
            IsLive = request.IsLive;
            LeagueGroups = leagueGroups;
        }

        public string CustomerClass { get; }

        public string CustomerClassDetail { get; }

        public string SportTypes { get; }

        public string LeagueGroup { get; }

        public string LeagueGroups { get; }

        public byte IsLive { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassRuleSearch";
    }
}
