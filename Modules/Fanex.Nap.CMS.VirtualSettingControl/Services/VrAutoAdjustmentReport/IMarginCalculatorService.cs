namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VrAutoAdjustmentReport
{
    using System.Collections.Generic;
    using Models.VrAutoAdjustmentReport;

    public interface IMarginCalculatorService
    {
        double CalculateTurnover(IEnumerable<CustomerMarginHistory> entryDetails);

        double CalculateWinloss(IEnumerable<CustomerMarginHistory> entryDetails);

        double CalculateMargin(double turnover, double winloss);
    }
}