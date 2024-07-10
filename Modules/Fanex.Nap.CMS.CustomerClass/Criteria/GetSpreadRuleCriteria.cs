namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;
    
    internal class GetSpreadRuleCriteria : QueryCriteriaBase
    {
        public GetSpreadRuleCriteria(SearchSpreadRuleRequest request, string sportTypes, string leagueGroups, byte type)
        {
            CustomerClasses = request.CustomerClass;
            Sites = request.Site;
            Currencies = request.Currency;
            LeagueCheck = request.LeagueGroups;
            LeagueGroups = leagueGroups;
            if (float.TryParse(request.Spread, out var spread)) Spread = spread;
            SportTypes = sportTypes;
            Type = type;
        }

        public string CustomerClasses { get; }
        public string SportTypes { get; }
        public string Sites { get; }
        public string Currencies { get; }
        public string LeagueGroups { get; }
        public string LeagueCheck { get; }
        public float? Spread { get; }
        public byte Type { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassSpreadSel";
    }
}
