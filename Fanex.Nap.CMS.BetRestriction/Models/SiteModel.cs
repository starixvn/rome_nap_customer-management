namespace Fanex.Nap.CMS.BetRestriction.Models
{
    public class SiteModel
    {
        public int SiteId { get; set; }

        public string Site { get; set; }

        public short? LiveBetAccept { get; set; }

        public byte? SeizeBetTicket { get; set; }

        public string FollowAccount { get; set; }

        public decimal? LeastStake { get; set; }

        public bool AccumulateBalance { get; set; }
    }
}
