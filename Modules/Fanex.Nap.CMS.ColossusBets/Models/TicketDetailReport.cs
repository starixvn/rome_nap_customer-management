namespace Fanex.Nap.CMS.ColossusBets.Models
{
    using System.Collections.Generic;

    public class TicketDetailReport
    {
        public string Own { get; set; }

        public string Sold { get; set; }

        public IList<TicketDetail> TicketDetails { get; set; }

        public decimal TotalWinLoss { get; set; }
    }
}
