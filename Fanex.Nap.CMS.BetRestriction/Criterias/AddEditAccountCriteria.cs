namespace Fanex.Nap.CMS.BetRestriction.Criterias
{
    using Fanex.Data.Repository;

    public abstract class AddEditAccountCriteria : CriteriaBase
    {
        public short? LiveBetAccept { get; set; }

        public int? SeizeBetTicket { get; set; }

        public string LeagueGroupId { get; set; }

        public decimal? MaxBet { get; set; }

        public decimal? MaxPerMatch { get; set; }

        public string FollowAccount { get; set; }

        public byte AccumulateBalance { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }
    }
}
