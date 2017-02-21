namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor
{
    using System;

    [Serializable]
    public class MarginModel
    {
        public MarginModel()
        {
        }

        public MarginModel(int month, int year)
        {
            Month = month;
            Year = year;
            Winloss = 0;
            Turnover = 0;
        }

        public MarginModel(int month, int year, decimal turnover, decimal winloss)
        {
            Month = month;
            Year = year;
            Winloss = winloss;
            Turnover = turnover;
        }

        public MarginModel(int month, int year, decimal turnover, decimal winloss, int betCount)
        {
            Month = month;
            Year = year;
            Winloss = winloss;
            BetCount = betCount;
            Turnover = turnover;
        }

        public int CustId { get; set; }

        public string Username { get; set; }

        public int BetCount { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public decimal Winloss { get; set; }

        public decimal Turnover { get; set; }

        public decimal Margin
        {
            get
            {
                return Turnover == 0 ? 0 : Math.Round(Winloss / Turnover * 100, 2);
            }
        }

        public DateTime ReportDate
        {
            get
            {
                try
                {
                    return new DateTime(Year, Month, 1);
                }
                catch
                {
                    return DateTime.Now;
                }
            }
        }
    }
}
