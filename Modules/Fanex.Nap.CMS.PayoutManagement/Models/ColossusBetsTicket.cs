namespace Fanex.Nap.CMS.PayoutManagement.Models
{
    using System.Collections.Generic;

    public class ColossusBetsTicket : BetTicket
    {
        public string Own { get; set; }

        public string Sold { get; set; }

        public IList<BetTicketDetail> BetTicketDetails { get; set; }

        public decimal TotalWinLoss { get; set; }
    }
}
