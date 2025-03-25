namespace Fanex.NAP.CMS.Models.StatusLog
{
    using System;

    public class StatusLogItem
    {
        public long LogID { get; set; }

        public DateTime LogDate { get; set; }

        public string LogDateShortDate => LogDate.ToString("MM/dd/yyyy");

        public string LogDateLongTime => LogDate.ToString("hh:mm:ss tt");

        public int EditorId { get; set; }

        public string EditorName { get; set; }

        public string Action { get; set; }

        public string SPName { get; set; }
        public string TrackingValue { get; set; }
        public string TrackingValue2 { get; set; }

        public string NewCollStatus { get; set; }

        public string OldCollStatus { get; set; }
    }
}
