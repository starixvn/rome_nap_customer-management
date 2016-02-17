namespace Fanex.Nap.CMS.ColossusBets.Models
{
    public class BonusTicketDetail : BonusTicket
    {
        public int CustId { get; set; }

        public string Result { get; set; }

        public int StatusId { get; set; }

        public decimal WinAmount { get; set; }

        public string TranId3rd { get; set; }

        public string EventId { get; set; }
    }
}
