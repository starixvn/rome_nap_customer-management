namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails
{
    using System;
    using System.Collections.Generic;

    public class VirtualRateLog
    {
        public string Username { get; set; }

        public string Editor { get; set; }

        public DateTime UpdatedDate { get; set; }

        public IEnumerable<VirtualRateDetailsByGroup> NewSports { get; set; }

        public IEnumerable<VirtualRateDetailsByGroup> OldSports { get; set; }

        public decimal ActualRate { get; set; }

        public string UpdatedDateText => UpdatedDate.ToString("MMM dd, yyyy HH:mm");
    }
}
