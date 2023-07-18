namespace Fanex.Nap.CMS.BetRestriction.Requests
{
    public class SiteRequest
    {
        public int SiteId { get; set; }

        public int UserId { get; set; }

        public short? LiveBetAccept { get; set; }

        public int? SeizeBetTicket { get; set; }

        public string FollowAccount { get; set; }

        public byte AccumulateBalance { get; set; }
    }
}
