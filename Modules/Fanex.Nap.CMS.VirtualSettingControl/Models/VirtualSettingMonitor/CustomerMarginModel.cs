namespace Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CustomerMarginModel
    {
        public CustomerMarginModel()
        {
            MarginList = new List<MarginModel>();
        }

        public string Username { get; set; }

        public int? OldRate { get; set; }

        public int? NewRate { get; set; }

        public bool? IsBA { get; set; }

        public List<MarginModel> MarginList { get; set; }

        public int Months
        {
            get
            {
                return MarginList.Where(t => t.Winloss != 0).Count();
            }
        }

        public int WonMonths
        {
            get
            {
                return MarginList.Where(t => t.Winloss > 0).Count();
            }
        }

        public MarginModel Last3Months
        {
            get
            {
                var last3Months = MarginList.Where(t => t.Turnover != 0).OrderByDescending(t => t.ReportDate).Take(3);
                var result = new MarginModel
                {
                    BetCount = last3Months.Sum(t => t.BetCount),
                    Turnover = last3Months.Sum(t => t.Turnover),
                    Winloss = last3Months.Sum(t => t.Winloss)
                };

                return result;
            }
        }

        public MarginModel Last6Months
        {
            get
            {
                var result = new MarginModel();

                if (MarginList.Where(t => t.Turnover != 0).Count() >= 6)
                {
                    var last6Months = MarginList.Where(t => t.Turnover != 0).OrderByDescending(t => t.ReportDate).Take(6);

                    result.BetCount = last6Months.Sum(t => t.BetCount);
                    result.Turnover = last6Months.Sum(t => t.Turnover);
                    result.Winloss = last6Months.Sum(t => t.Winloss);
                }

                return result;
            }
        }

        public MarginModel Total
        {
            get
            {
                var result = new MarginModel
                {
                    BetCount = MarginList.Sum(t => t.BetCount),
                    Turnover = MarginList.Sum(t => t.Turnover),
                    Winloss = MarginList.Sum(t => t.Winloss)
                };

                return result;
            }
        }
    }
}
