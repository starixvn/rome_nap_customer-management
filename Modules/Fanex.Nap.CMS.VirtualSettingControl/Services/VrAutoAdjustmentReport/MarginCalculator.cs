namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VrAutoAdjustmentReport
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.VrAutoAdjustmentReport;

    public class MarginCalculator : IMarginCalculatorService
    {
        public double CalculateTurnover(IEnumerable<CustomerMarginHistory> entryDetails)
        {
            return entryDetails.Average(l => l.Turnover);
        }

        public double CalculateWinloss(IEnumerable<CustomerMarginHistory> entryDetails)
        {
            return entryDetails.Average(l => l.Winloss);
        }

        public double CalculateMargin(double turnover, double winloss)
        {
            return turnover == 0 ? 0 : (winloss / turnover);
        }
    }
}