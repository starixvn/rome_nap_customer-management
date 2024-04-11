namespace Fanex.Nap.CMS.CustomerClass.Models
{
    internal class Rule
    {
#pragma warning disable S107
        public Rule(
            int ruleId,
            short? customerClass,
            short? customerClassDetail,
            string ruleTitle,
            short sportType,
            string leagueGroupIds,
            string leagueGroups,
            byte isLive,
            int? diffBalanceRatio,
            int? betDelay,
            short? nextBetInterval,
            short? superBetInterval,
            short? siteBetInterval,
            int? minBet,
            int? maxBet,
            int? maxPerMatch,
            byte? repeatedBets,
            byte? seizeBet,
            bool disableParlay,
            short? peakHourSpread,
            bool? enable,
            string site,
            string currencies,
            string platforms,
            string policyTitles)
#pragma warning restore S107
        {
            RuleId = ruleId;
            CustomerClass = customerClass == 0 ? null : customerClass;
            CustomerClassDetail = customerClassDetail == 0 ? null : customerClassDetail;
            RuleTitle = ruleTitle;
            SportType = sportType;
            LeagueGroupIds = leagueGroupIds;
            LeagueGroups = leagueGroups;
            IsLive = isLive;
            DiffBalanceRatio = diffBalanceRatio;
            BetDelay = betDelay;
            NextBetInterval = nextBetInterval;
            SuperBetInterval = superBetInterval;
            SiteBetInterval = siteBetInterval;
            MinBet = minBet;
            MaxBet = maxBet;
            MaxPerMatch = maxPerMatch;
            RepeatedBets = repeatedBets;
            SeizeBet = seizeBet;
            DisableParlay = disableParlay;
            PeakHourSpread = peakHourSpread;
            Enable = enable;
            Site = site;
            Currencies = currencies;
            Platforms = platforms;
            PolicyTitles = policyTitles;
        }

        public int RuleId { get; }

        public short? CustomerClass { get; }

        public short? CustomerClassDetail { get; }

        public string RuleTitle { get; }

        public short SportType { get; }

        public string SportTypeName { get; set; }

        public string LeagueGroupIds { get; }

        public string LeagueGroups { get; }

        public bool? IsAllLeagueGroups { get; set; }

        public byte IsLive { get; }

        public int? DiffBalanceRatio { get; }

        public int? BetDelay { get; }

        public short? NextBetInterval { get; }

        public short? SuperBetInterval { get; }

        public short? SiteBetInterval { get; }

        public int? MinBet { get; }

        public int? MaxBet { get; }

        public int? MaxPerMatch { get; }

        public byte? RepeatedBets { get; }

        public byte? SeizeBet { get; }

        public bool DisableParlay { get; }

        public short? PeakHourSpread { get; }

        public bool? Enable { get; }

        public string Site { get; }

        public string Currencies { get; set; }

        public bool? IsAllCurrencies { get; set; }

        public string Platforms { get; }

        public string PlatformNames { get; set; }

        public bool? IsAllPlatforms { get; set; }

        public string PolicyTitles { get; set; }
    }
}
