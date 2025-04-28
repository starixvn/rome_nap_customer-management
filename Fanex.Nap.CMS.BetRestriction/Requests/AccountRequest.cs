namespace Fanex.Nap.CMS.BetRestriction.Requests
{
    public class AccountRequest
    {
        public int CustId { get; set; }

        public string UserName { get; set; }

        public short? LiveBetAccept { get; set; }

        public int? SeizeBetTicket { get; set; }

        public string LeagueGroupId { get; set; }

        public decimal? MaxBet { get; set; }

        public decimal? MaxPerMatch { get; set; }

        public string FollowAccount { get; set; }

        public decimal? LeastStake { get; set; }

        public byte AccumulateBalance { get; set; }
    }
}
