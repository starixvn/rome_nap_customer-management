using System.Security.Policy;

namespace Fanex.NAP.CMS.BetLimitControl.Request
{
    public class AddRequest
    {
        public string Username { get; set; }

        public int BetLimitTypeID { get; set; }

        public byte Mode { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public string Permission { get; set; }
    }
}