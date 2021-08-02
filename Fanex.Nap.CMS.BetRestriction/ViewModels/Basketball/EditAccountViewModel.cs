namespace Fanex.Nap.CMS.BetRestriction.ViewModels.Basketball
{
    public class EditAccountViewModel
    {
        public int CustId { get; set; }

        public string Account { get; set; }

        public short? LiveBetAccept { get; set; }

        public byte? SeizeBetTicket { get; set; }

        public int? FollowAccountID { get; set; }

        public string FollowAccount { get; set; }
    }
}
