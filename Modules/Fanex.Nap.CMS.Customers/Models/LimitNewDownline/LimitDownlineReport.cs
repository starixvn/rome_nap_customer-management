using System.Collections.Generic;

namespace Fanex.Nap.CMS.Customers.Models.LimitNewDownline
{
    public class LimitDownlineReport
    {
        public LimitDownlineReport(IEnumerable<LimitDownlineItem> limitDownlineList, int totalItems)
        {
            LimitDownlineList = limitDownlineList;
            TotalItems = totalItems;
        }

        public IEnumerable<LimitDownlineItem> LimitDownlineList { get; }

        public int TotalItems { get; set; }
    }
}
