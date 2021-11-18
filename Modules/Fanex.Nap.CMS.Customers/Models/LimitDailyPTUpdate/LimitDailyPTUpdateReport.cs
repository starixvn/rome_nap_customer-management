namespace Fanex.Nap.CMS.Customers.Models.LimitDailyPTUpdate
{
    using System.Collections.Generic;

    public class LimitDailyPTUpdateReport
    {
        public LimitDailyPTUpdateReport(IEnumerable<LimitDailyPTUpdateItem> limitDailyPTUpdateList, int totalItems)
        {
            LimitDailyPTUpdateList = limitDailyPTUpdateList;
            TotalItems = totalItems;
        }

        public IEnumerable<LimitDailyPTUpdateItem> LimitDailyPTUpdateList { get; }

        public int TotalItems { get; set; }
    }
}
