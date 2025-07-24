namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class GetSpreadRuleCriteria : QueryCriteriaBase
    {
        public string CustomerClasses { get; set; }
        public string SportTypes { get; set; }
        public string Sites { get; set; }
        public string Currencies { get; set; }
        public string LeagueGroups { get; set; }
        public string LeagueCheck { get; set; }
        public float? Spread { get; set; }
        public byte Type { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_CustomerClassSpreadSel";
    }
}
