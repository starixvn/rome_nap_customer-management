namespace Fanex.NAP.CMS.Models.CreditCollectionSummary
{
    using System.Collections.Generic;
    using System.Linq;

    public class CreditCollectionSummaryItem
    {
        public int CustId { get; set; }

        public string Code { get; set; }

        public int AccountType { get; set; }

        public int GroupId { get; set; }

        public int Status { get; set; }

        public decimal LastBalance { get; set; }

        public decimal CurrentBalance { get; set; }

        public int HeadlineId { get; set; }

        public List<decimal> BreakDowns { get; set; }

        public List<CreditCollectionSummaryItem> Downlines { get; set; }

        public decimal NetMovement => CurrentBalance - LastBalance;

        public decimal DebtMovement => NetMovement - Payment;

        public decimal Payment => BreakDowns.Sum();
    }
}
