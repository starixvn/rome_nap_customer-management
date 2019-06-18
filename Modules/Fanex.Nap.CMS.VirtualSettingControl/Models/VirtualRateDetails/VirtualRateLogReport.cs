namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    using System;
    using System.Collections.Generic;

    public class VirtualRateLogReport
    {
        public int ResultCode { get; set; }

        public string ResultMessage { get; set; }

        public string Username { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public bool IncludeUpline { get; set; }

        public string Currency { get; set; }

        public string FromDateText => FromDate.ToString("MMM dd, yyyy");

        public string ToDateText => ToDate.ToString("MMM dd, yyyy");

        public IEnumerable<VirtualRateLog> Logs { get; set; }
    }
}
