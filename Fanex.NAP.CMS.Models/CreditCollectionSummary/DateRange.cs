namespace Fanex.NAP.CMS.Models.CreditCollectionSummary
{
    using System;

    public class DateRange
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string FromDateStr => FromDate.ToString("MMM dd, yyyy");

        public string ToDateStr => ToDate.ToString("MMM dd, yyyy");
    }
}
