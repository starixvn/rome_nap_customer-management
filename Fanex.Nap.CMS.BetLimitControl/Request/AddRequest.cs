namespace Fanex.NAP.CMS.BetLimitControl.Request
{
    public class AddRequest
    {
        public string Username { get; set; }

        public int BetLimitTypeID { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }
    }
}