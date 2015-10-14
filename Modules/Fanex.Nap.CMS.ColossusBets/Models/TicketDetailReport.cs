namespace Fanex.Nap.CMS.ColossusBets.Models
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.ColossusBets.Models.API;

    public class TicketDetailReport
    {
        public string Own { get; set; }

        public string Sold { get; set; }

        public IList<TicketDetail> TicketDetails { get; set; }

        public decimal TotalWinLoss { get; set; }

        public long BetId { get; set; }

        public ColossusBetsAuthorization CBAuthorization { get; set; }
    }
}
