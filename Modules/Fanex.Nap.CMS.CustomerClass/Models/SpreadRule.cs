namespace Fanex.Nap.CMS.CustomerClass.Models
{
    using System;
    
    public class SpreadRule
    {
        public SpreadRule(int policyID, string policyTitle, short sportType, decimal spread, string customerClass, string currencies, string sites, string platforms, bool enable, byte isTest, int lastModifiedUser, DateTime lastModifiedTime, short isLive, string leagueGroups)
        {
            PolicyID = policyID;
            PolicyTitle = policyTitle;
            SportType = sportType;
            Spread = spread;
            CustomerClass = customerClass;
            Currencies = currencies;
            Sites = sites;
            Platforms = platforms;
            LeagueGroups = leagueGroups;
            Enable = enable;
            IsLive = isLive;
            IsTest = isTest;
            LastModifiedUser = lastModifiedUser;
            LastModifiedTime = lastModifiedTime;
        }

        public int PolicyID { get; }
        public string PolicyTitle { get; }
        public decimal Spread { get; }
        public short SportType { get; }
        public string CustomerClass { get; }
        public string Currencies { get; }
        public string Sites { get; }
        public string Platforms { get; }
        public string LeagueGroups { get; }
        public bool Enable { get; }
        public short IsLive { get; }
        public byte IsTest { get; }
        public int LastModifiedUser { get; }
        public DateTime LastModifiedTime { get; }
    }
}
