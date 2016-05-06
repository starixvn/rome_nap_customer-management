namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor
{
    public class VirtualSettingModel
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public decimal VRate { get; set; }

        public decimal Winloss { get; set; }

        public decimal Turnover { get; set; }

        public decimal Margin { get; set; }

        public int ReportYear { get; set; }

        public int ReportMonth { get; set; }
    }
}
