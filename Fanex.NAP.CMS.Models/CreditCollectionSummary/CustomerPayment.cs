namespace Fanex.NAP.CMS.Models.CreditCollectionSummary
{
    using System;

    public class CustomerPayment
    {
        public int GroupId { get; set; }

        public string Code { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Payment { get; set; }
    }
}
