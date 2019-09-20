namespace Fanex.Nap.CMS.BetRestriction.ViewModels
{
    public class EditSiteViewModel
    {
        public int SiteId { get; set; }

        public string Site { get; set; }

        public short? LiveBetAccept { get; set; }

        public byte? SeizeBetTicket { get; set; }

        public string FollowAccount { get; set; }
    }
}
