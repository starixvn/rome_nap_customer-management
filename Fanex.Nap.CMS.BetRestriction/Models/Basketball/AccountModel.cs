namespace Fanex.Nap.CMS.BetRestriction.Models.Basketball
{
    using Fanex.Nap.CMS.Infrastructure.Constants;

    public class AccountModel
    {
        public int CustId { get; set; }

        public string Account { get; set; }

        public int RoleId { get; set; }

        public string Level => RoleIdConstants.GetRoleName(RoleId);

        public string Status { get; set; }

        public string Site { get; set; }

        public int SiteId { get; set; }

        public short? LiveBetAccept { get; set; }

        public byte? SeizeBetTicket { get; set; }

        public string Currency { get; set; }

        public string ProblemAccount { get; set; }

        public string FollowAccount { get; set; }
    }
}
