namespace Fanex.Nap.CMS.Customers.Models.VirtualRateEngine
{
    internal class CheckVREngineRuleResult
    {
        public int Sporttype { get; set; }
        public int Bettype { get; set; }
        public int LeagueGroupID { get; set; }
        public int LeagueID { get; set; }
        public int SiteID { get; set; }
        public int CustomerClass { get; set; }
        public int DateOfWeek { get; set; }
        public int CurrencyID { get; set; }
        public byte IsApply { get; set; }
        public int CustomerClassDetail { get; set; }
        public int CCSporttype { get; set; }
    }
}
