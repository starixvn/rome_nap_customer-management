namespace Fanex.NAP.CMS.Models.CreditCollectionSummary
{
    using System;
    using System.Collections.Generic;

    public class CreditCollectionSummaryReport
    {
        public DateTime LastBalanceDate { get; set; }

        public DateTime CurrentBalanceDate { get; set; }

        public IEnumerable<DateRange> BreakDowns { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRows { get; set; }

        public List<CreditCollectionSummaryItem> Groups { get; set; }

        public string LastBalanceDateStr => LastBalanceDate.ToString("MMM dd, yyyy");

        public string CurrentBalanceDateStr => CurrentBalanceDate.ToString("MMM dd, yyyy");
    }
}
