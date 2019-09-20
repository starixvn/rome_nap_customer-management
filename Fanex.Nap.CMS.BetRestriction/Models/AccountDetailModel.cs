namespace Fanex.Nap.CMS.BetRestriction.Models
{
    using Fanex.Nap.CMS.Infrastructure.Constants;

    public class AccountDetailModel
    {
        public int CustId { get; set; }

        public string Account { get; set; }

        public short? LiveBetAccept { get; set; }

        public int? SeizeBetTicket { get; set; }

        public decimal? MaxBet { get; set; }

        public decimal? MaxPerMatch { get; set; }

        public string FollowAccount { get; set; }
    }
}
