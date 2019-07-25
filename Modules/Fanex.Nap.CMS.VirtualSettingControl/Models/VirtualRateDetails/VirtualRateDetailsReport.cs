namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    using System.Collections.Generic;

    public class VirtualRateDetailsReport
    {
        public int ResultCode { get; set; }

        public string ResultMessage { get; set; }

        public string UplineCustName { get; set; }

        public int UplineCustId { get; set; }

        public int ReportLevel { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public bool EnableNonLive => true;

        public bool EnableLive => true;

        public IEnumerable<VirtualRateDetailsByGroup> Sports { get; set; }

        public IEnumerable<RateInfo> VirtualRateValueRange { get; set; }

        public string Currency { get; set; }

        public decimal ActualRate { get; set; }
    }
}
