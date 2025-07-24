namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class AddSpreadRuleCriteria : CustomerClassCommandBaseCriteria
    {
        public int PolicyID { get; set; }
        public string PolicyTitle { get; set; }
        public int SportType { get; set; }
        public decimal Spread { get; set; }
        public string CustomerClasses { get; set; }
        public string Currencies { get; set; }
        public string Sites { get; set; }
        public string Platforms { get; set; }
        public bool Enable { get; set; }
        public string LeagueGroups { get; set; }
        public byte IsLive { get; set; }
        public byte Type { get; set; }

        public override string GetSettingKey()
            => "NAP_CM_CustomerClassSpreadIns";
    }
}
