namespace Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitorAdvance
{
    using System;

    public class GetVirtualSettingMonitorAdvanceCriteria
    {
        public int SportId { get; set; }

        public int Rate { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int Depth { get; set; }

        public string Username { get; set; }
    }
}
