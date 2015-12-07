namespace Fanex.Nap.CMS.PayoutManagement.Criteria
{
    using System;
    using System.Collections;
    using System.Globalization;
    using Fanex.Data.Repository;

    public class GetPayoutListCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public string Site { get; set; }

        public string Product { get; set; }

        public string Status { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_PayoutTrans_SelList";
        }

        public override Hashtable GetSpParams()
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            var fromDate = DateTime.ParseExact(From, "MM/dd/yyyy", provider);
            var toDate = DateTime.ParseExact(To, "MM/dd/yyyy", provider);
            return new Hashtable 
            {
                { "Username", string.IsNullOrEmpty(Username) ? string.Empty : Username },
                { "SiteIds", Site },
                { "ProductIds", Product },
                { "PayoutStatusIds", Status },
                { "FromDate", new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0) },
                { "ToDate", new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59) }
            };
        }
    }
}
