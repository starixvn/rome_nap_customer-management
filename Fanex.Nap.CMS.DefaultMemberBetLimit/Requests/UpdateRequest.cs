namespace Fanex.NAP.CMS.DefaultMemberBetLimit.Requests
{
    public class UpdateRequest
    {
        public int BetLimitTypeId { get; set; }

        public long CustId { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal MaxBet { get; set; }
    }
}
