namespace Fanex.Nap.CMS.CustomerClass.Models
{
    internal class Rule
    {
        public int RuleId { get; set; }

        public short? CustomerClass { get; set; }

        public short? CustomerClassDetail { get; set; }

        public string RuleTitle { get; set; }

        public short SportType { get; set; }

        public string SportTypeName { get; set; }

        public string LeagueGroupIds { get; set; }

        public string LeagueGroups { get; set; } = string.Empty;

        public bool? IsAllLeagueGroups { get; set; }

        public byte IsLive { get; set; }

        public int? DiffBalanceRatio { get; set; }

        public int? BetDelay { get; set; }

        public short? NextBetInterval { get; set; }

        public short? SuperBetInterval { get; set; }

        public short? SiteBetInterval { get; set; }

        public int? MinBet { get; set; }

        public int? MaxBet { get; set; }

        public int? MaxPerMatch { get; set; }

        public byte? RepeatedBets { get; set; }

        public byte? SeizeBet { get; set; }

        public bool DisableParlay { get; set; }

        public short? PeakHourSpread { get; set; }

        public bool? Enable { get; set; }

        public bool? TopPriority { get; set; }

        public string Site { get; set; }

        public string Currencies { get; set; }

        public bool? IsAllCurrencies { get; set; }

        public string Platforms { get; set; }

        public string PlatformNames { get; set; }

        public bool? IsAllPlatforms { get; set; }

        public string PolicyTitles { get; set; } = string.Empty;
    }
}
