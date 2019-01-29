namespace Fanex.NAP.CMS.Models.StatusLog
{
    using System.Collections.Generic;

    public class StatusLogReport
    {
        public int TotalRows { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public IEnumerable<StatusLogItem> Logs { get; set; }
    }
}
