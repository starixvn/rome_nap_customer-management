using System;

namespace Fanex.NAP.CMS.ViewModels.ManageSmartPunters
{
    public class GetActionMonitoringLogsFilter
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string UserName { get; set; }

        public string Admin { get; set; }

        public string SortField { get; set; }

        public string SortDirection { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
