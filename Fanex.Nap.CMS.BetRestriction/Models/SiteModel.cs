namespace Fanex.Nap.CMS.BetRestriction.Models
{
    public class SiteModel
    {
        public int SiteId { get; set; }

        public string Site { get; set; }

        public int LiveBetAccept { get; set; }

        public int SeizeBetTicket { get; set; }

        public string FollowAccount { get; set; }
    }
}
