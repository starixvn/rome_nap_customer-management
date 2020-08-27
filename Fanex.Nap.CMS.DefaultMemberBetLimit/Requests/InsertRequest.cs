namespace Fanex.NAP.CMS.DefaultMemberBetLimit.Requests
{
    public class InsertRequest
    {
        public int BetLimitTypeId { get; set; }

        public string Username { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal MaxBet { get; set; }
    }
}
