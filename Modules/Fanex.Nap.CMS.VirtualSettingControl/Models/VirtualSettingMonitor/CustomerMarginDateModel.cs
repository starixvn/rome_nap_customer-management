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
                return AdjustDate != null ? AdjustDate.ToString("MM/dd/yyyy HH:mm:ss.fff") : string.Empty;
            }
        }

        public string AdjustDateValue
        {
            get
            {
                return AdjustDate != null ? AdjustDate.ToString("MM/dd/yyyy HH:mm:ss.fff") : string.Empty;
            }
        }
    }
}
