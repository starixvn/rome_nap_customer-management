namespace Fanex.Nap.CMS.ColossusBets.Models
{
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class Ticket
    {
        public int Number { get; set; }

        public IdNameModel Member { get; set; }

        public IdNameModel Currency { get; set; }

        public string Site { get; set; }

        public long BetId { get; set; }

        public decimal TotalPayout { get; set; }

        public IdNameModel Status { get; set; }

        public string SecurityCode { get; set; }

        public bool HasNotes { get; set; }
    }
}
