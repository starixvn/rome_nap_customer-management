namespace Fanex.Nap.CMS.VirtualSettingControl.Models.CustomerDashboard
{
    using System;

    /// <summary>
    /// Summary Report BetType Performance model.
    /// </summary>
    public class BetTypePerformance
    {
        public int BetType { get; set; }

        public string BetTypeName { get; set; }

        public decimal Turnover { get; set; }

        public decimal Winloss { get; set; }

        public decimal? Margin { get; set; }
    }
}
