namespace Fanex.NAP.CMS.BetLimitControl.Request
{
    public class UpdateRequest
    {
        public int CustID { get; set; }

        public int BetLimitTypeID { get; set; }

        public int NewBetLimitTypeID { get; set; }

        public byte Mode { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }
    }
}