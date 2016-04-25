namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VrAutoAdjustmentReport
{
    using System;
    using System.Collections.Generic;
    using Data.Repository;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Models.VrAutoAdjustmentReport;

    public interface IAdjustmentReportService
    {
        IEnumerable<VrAutoAdjustmentLogEntry> GetReport(ICriteria criteria);

        List<MarginModel> GetCustomerVirtualSettingLatestMonths(int custId, DateTime reportDate, int monthsToCheck);
    }
}