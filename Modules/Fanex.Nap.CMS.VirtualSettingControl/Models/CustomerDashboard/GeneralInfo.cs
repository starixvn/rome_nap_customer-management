namespace Fanex.Nap.CMS.VirtualSettingControl.Models.CustomerDashboard
{
    using System;

    /// <summary>
    /// Summary Report General Information.
    /// </summary>
    public class GeneralInfo
    {
        public decimal Turnover { get; set; }

        public decimal Winloss { get; set; }

        public decimal? Margin { get; set; }

        public int BetCount { get; set; }
    }
}
