namespace Fanex.Nap.CMS.ColossusBets.Models
{
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class Ticket
    {
        public int Id { get; set; }

        public IdNameModel Member { get; set; }

        public IdNameModel Currency { get; set; }

        public IdNameModel Site { get; set; }

        public string BetId { get; set; }

        public decimal TotalPayout { get; set; }

        public string Status { get; set; }

        public string SecurityCode { get; set; }
    }
}
