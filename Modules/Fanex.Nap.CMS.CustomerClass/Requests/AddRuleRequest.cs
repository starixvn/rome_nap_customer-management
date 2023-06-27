namespace Fanex.Nap.CMS.CustomerClass.Requests
{
    using Fanex.Nap.CMS.Infrastructure.Requests;

    public class AddRuleRequest : CommandBase
    {
        public CustomerClassRuleTitleRequest[] CCRuleTitles { get; set; }

        public byte SportType { get; set; }

        public string LeagueGroups { get; set; }

        public byte IsLive { get; set; }

        public int? DiffBalanceRatio { get; set; }

        public int? BetDelay { get; set; }

        public int? NextBetIntervalSec { get; set; }

        public int? SuperBetIntervalSec { get; set; }

        public int? SiteBetIntervalSec { get; set; }

        public int? MinBet { get; set; }

        public int? MaxBet { get; set; }

        public int? MaxPerMatch { get; set; }

        public int? RepeatedBets { get; set; }

        public int? SeizeBet { get; set; }

        public bool SeizeBetAccumulateBal { get; set; }

        public bool EnableParlay { get; set; }

        public int? PeakHourSpread { get; set; }

#pragma warning disable S1067
        public override bool IsValid()
            => base.IsValid()
            && CCRuleTitles != null && CCRuleTitles.Length > 0
            && SportType > 0
            && !string.IsNullOrWhiteSpace(LeagueGroups);
#pragma warning restore S1067
    }
}
