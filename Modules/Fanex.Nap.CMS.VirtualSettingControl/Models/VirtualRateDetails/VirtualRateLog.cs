namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    using System;
    using System.Collections.Generic;

    public class VirtualRateLog
    {
        public string LogId { get; set; }

        public string Username { get; set; }

        public decimal ExchangeRate { get; set; }

        public string ExchangeRateText { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string Editor { get; set; }

        public IEnumerable<VirtualRateDetailsByGroup> Sports { get; set; }

        public string UpdatedDateText => UpdatedDate.ToString("MMM dd, yyyy HH:mm");
    }
}
