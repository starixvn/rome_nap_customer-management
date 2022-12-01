namespace Fanex.Nap.CMS.CustomerClass.Requests
{
    public class SearchRuleRequest
    {
        public short CustomerClass { get; set; }

        public short SportType { get; set; }

        public string LeagueGroups { get; set; }

        public byte IsLive { get; set; }
    }
}
