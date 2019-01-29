namespace Fanex.NAP.CMS.Models.CreditCollectionSummary
{
    using System;

    public class CustomerBalance
    {
        public int GroupId { get; set; }

        public string Code { get; set; }

        public decimal Balance { get; set; }

        public DateTime WinLossDate { get; set; }
    }
}
