namespace Fanex.Nap.CMS.BetRestriction.Models
{
    using Fanex.Nap.CMS.Infrastructure.Constants;

    public class AccountModel
    {
        public int CustId { get; set; }

        public string Headline { get; set; }

        public string Account { get; set; }

        public int RoleId { get; set; }

        public string Level => RoleIdConstants.GetRoleName(RoleId);

        public string Status { get; set; }

        public string Site { get; set; }

        public int SiteId { get; set; }

        public int LiveBetAccept { get; set; }

        public byte? SeizeBetTicket { get; set; }

        public string Currency { get; set; }

        public decimal? MaxBet { get; set; }

        public decimal? MaxPerMatch { get; set; }

        public string ProblemAccount { get; set; }

        public string FollowAccount { get; set; }
    }
}
