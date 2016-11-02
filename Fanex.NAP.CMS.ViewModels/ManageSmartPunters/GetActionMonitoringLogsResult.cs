using System.Collections.Generic;

namespace Fanex.NAP.CMS.ViewModels.ManageSmartPunters
{
    public class GetActionMonitoringLogsResult
    {
        public IList<ActionMonitoringLog> ActionMonitoringLogs { get; set; }

        public int TotalItems { get; set; }
    }
}
