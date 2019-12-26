namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    using System;
    using System.Collections.Generic;

    public class AutoSuspendLogReport
    {
        public int ResultCode { get; set; }

        public string ResultMessage { get; set; }

        public string Username { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string FromDateText => FromDate.ToString("MMM dd, yyyy");

        public string ToDateText => ToDate.ToString("MMM dd, yyyy");

        public IEnumerable<AutoSuspendLog> Logs { get; set; }
    }
}
