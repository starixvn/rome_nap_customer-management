namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VrAutoAdjustmentReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Models.VrAutoAdjustmentReport;

    public interface IAdjustmentReportService
    {
        IEnumerable<VrAutoAdjustmentLogEntry> GetReport(ICriteria criteria);

        ILookup<int, MarginModel> GetVirtualSettingLatestMonths(IEnumerable<int> custIds, DateTime reportDate, int monthsToCheck);
    }
}