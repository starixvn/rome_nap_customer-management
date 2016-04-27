namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VrAutoAdjustmentReport
{
    using System;
    using System.Collections.Generic;
    using Data.Repository;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Utility;
    using Models.VrAutoAdjustmentReport;

    public interface IAdjustmentReportService
    {
        QueryResult<VrAutoAdjustmentLogEntry> GetReport(PagingCriteria criteria);

        IEnumerable<MarginModel> GetVirtualSettingLatestMonths(int custId, DateTime reportDate, int monthsToCheck);
    }
}