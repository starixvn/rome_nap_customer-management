namespace Fanex.Nap.CMS.ColossusBets.Models
{
    using System;
    using Newtonsoft.Json;

    public class TicketDetail
    {
        [JsonIgnore]
        public DateTime TransDate { get; set; }

        public string Date { get; set; }

        [JsonIgnore]
        public int BetType { get; set; }

        public string TransactionType { get; set; }

        [JsonIgnore]
        public decimal Sold { get; set; }

        public decimal WinLoss { get; set; }

        public string Status { get; set; }
    }
}
