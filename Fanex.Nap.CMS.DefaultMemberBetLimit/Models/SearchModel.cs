namespace Fanex.NAP.CMS.DefaultMemberBetLimit.Models
{
    public class SearchModel
    {
        public int BetLimitTypeId { get; set; }

        public string BetLimitType { get; set; }

        public string Username { get; set; }

        public string Level { get; set; }

        public string Currency { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public long CustId { get; set; }
    }
}
