namespace Fanex.Nap.CMS.CustomerClass.Models
{
    internal class SpreadSetting
    {
        public string PolicyTitle { get; set; }
        public string CustomerClass { get; set; }
        public short SportType { get; set; }
        public string Site { get; set; }
        public string Currency { get; set; }
        public string Platform { get; set; }
        public decimal Spread { get; set; }
        public bool Enable { get; set; } = true;
        //public string LeagueGroups { get; set; }
        //public byte IsLive { get; set; }
    }
}
