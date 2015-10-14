namespace Fanex.Nap.CMS.ColossusBets.Models
{
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class TicketDetail
    {
        public string Date { get; set; }

        public int BetType { get; set; }

        public string TransactionType { get; set; }

        public decimal Sold { get; set; }

        public decimal WinLoss { get; set; }

        public string Status { get; set; }
    }
}
