namespace Fanex.Nap.CMS.VirtualSettingControl.Models.CustomerDashboard
{
    using System;

    /// <summary>
    /// Event Choice model.
    /// </summary>
    public class EventChoice
    {
        public bool IsLive { get; set; }

        public string LiveStatus
        {
            get
            {
                return IsLive ? "Live" : "Non-Live";
            }
        }

        public decimal Turnover { get; set; }

        public decimal Winloss { get; set; }

        public decimal? Margin { get; set; }
    }
}
