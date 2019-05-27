namespace Fanex.NAP.CMS.Models.CreditCollectionSummary
{
    using System;

    public class CreditCollectionSummaryFilter
    {
        public string Code { get; set; }

        public int AccountType { get; set; }

        public string Status { get; set; }

        public DateTime LastBalanceDate { get; set; }

        public DateTime CurrentBalanceDate { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}
