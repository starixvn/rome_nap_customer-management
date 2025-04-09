namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    using System.Collections.Generic;
    using Fanex.Common;

    public class VirtualRateRealValues
    {
        public int ResultCode { get; set; }

        public string ResultMessage { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public string Currency { get; set; }

        public decimal ExchangeRate { get; set; }

        public string ExchangeRateText { get; set; }

        public IEnumerable<VirtualRateDetailsByGroup> Childs { get; set; }
    }
}
