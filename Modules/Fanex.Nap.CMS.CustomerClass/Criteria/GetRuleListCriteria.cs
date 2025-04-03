namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class GetRuleListCriteria : QueryCriteriaBase
    {
        public string CustomerClass { get; set; }

        public string CustomerClassDetail { get; set; }

        public string SportTypes { get; set; }

        public string LeagueGroup { get; set; }

        public string LeagueGroups { get; set; }

        public byte IsLive { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_CustomerClassRuleSearch";
    }
}
