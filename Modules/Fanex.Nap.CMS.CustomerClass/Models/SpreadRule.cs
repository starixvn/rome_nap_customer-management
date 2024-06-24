namespace Fanex.Nap.CMS.CustomerClass.Models
{
    using System;
    
    public class SpreadRule
    {
        public int PolicyID { get; set; }
        public string PolicyTitle { get; set; }
        public decimal Spread { get; set; }
        public short SportType { get; set; }
        public string CustomerClass { get; set; }
        public string Currencies { get; set; }
        public string Sites { get; set; }
        public string Platforms { get; set; }
        public string LeagueGroups { get; }
        public bool Enable { get; set; }
        public short IsLive { get; }
        public byte IsTest { get; set; }
        public int LastModifiedUser { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
}
