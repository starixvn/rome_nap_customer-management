namespace Fanex.Nap.CMS.VirtualSettingControl.Models.CustomerDashboard
{
    using System;

    /// <summary>
    /// Summary Report Sport Performance model.
    /// </summary>
    public class SportPerformance
    {
        public int SportType { get; set; }

        public string SportName { get; set; }

        public decimal Turnover { get; set; }

        public decimal Winloss { get; set; }

        public decimal? Margin { get; set; }
    }
}
