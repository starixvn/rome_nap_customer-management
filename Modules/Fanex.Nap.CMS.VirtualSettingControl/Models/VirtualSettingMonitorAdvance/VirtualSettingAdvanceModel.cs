namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitorAdvance
{
    using System;

    [Serializable]
    public class VirtualSettingAdvanceModel
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public decimal Winloss { get; set; }

        public decimal Turnover { get; set; }

        public decimal? Margin
        {
            get { return Turnover != 0 ? (decimal?)Math.Round(Winloss / Turnover * 100, 2) : null; }
        }

        public int BetCount { get; set; }

        public int ReportYear { get; set; }

        public int ReportMonth { get; set; }

        public decimal VarianceWinloss
        {
            get
            {
                return Winloss - WinlossVirtual;
            }
        }

        public decimal VarianceTurnover
        {
            get
            {
                return Turnover - TurnoverVirtual;
            }
        }

        public decimal? VarianceMargin
        {
            get { return VarianceTurnover != 0 ? (decimal?)Math.Round(VarianceWinloss / VarianceTurnover * 100, 2) : null; }
        }

        public DateTime ReportDate
        {
            get { return ReportYear > 0 && ReportMonth > 0 ? new DateTime(ReportYear, ReportMonth, 1) : new DateTime(1, 1, 1); }
        }

        public decimal WinlossVirtual { get; set; }

        public decimal TurnoverVirtual { get; set; }
    }
}