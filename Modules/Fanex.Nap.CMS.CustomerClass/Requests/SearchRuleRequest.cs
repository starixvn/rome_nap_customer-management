namespace Fanex.Nap.CMS.CustomerClass.Requests
{
    public class SearchRuleRequest
    {
        public string CustomerClass { get; set; }

        public string CustomerClassDetail { get; set; }

        public short SportType { get; set; }

        public string LeagueGroups { get; set; }

        public byte IsLive { get; set; }
    }
}
