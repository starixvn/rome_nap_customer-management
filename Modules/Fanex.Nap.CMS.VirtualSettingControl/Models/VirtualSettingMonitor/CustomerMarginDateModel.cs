namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor
{
    using System;

    public class CustomerMarginDateModel
    {
        public DateTime AdjustDate { get; set; }

        public string AdjustDateString
        {
            get
            {
                return AdjustDate != null ? AdjustDate.ToString("MM/dd/yyyy hh:mm:ss.fff") : "";
            }
        }

        public string AdjustDateValue
        {
            get
            {
                return AdjustDate != null ? AdjustDate.ToString("MM/dd/yyyy hh:mm:ss.fff") : "";
            }
        }
    }
}
