namespace Fanex.Nap.CMS.PayoutManagement.Models
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Extensions;

    public class BetTicketDetail
    {
        public DateTime TransDate { get; set; }

        public int BetType { get; set; }

        public string TransactionType { get; set; }

        public decimal Sold { get; set; }

        public decimal WinLoss { get; set; }

        public string Status { get; set; }

        public string Date
        {
            get 
            {
                return TransDate.ToLongFormatedDateWithSlash();
            }
        }
    }
}
