namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VrAutoAdjustmentReport
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VrAutoAdjustmentReport;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Models.VrAutoAdjustmentReport;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface IAdjustmentReportService
    {
        IEnumerable<VrAutoAdjustmentLogEntry> GetReport(ICriteria criteria);

        ILookup<int, MarginModel> GetVirtualSettingLatestMonths(IEnumerable<int> custIds, DateTime reportDate, int monthsToCheck);

        void ExportVRAdjustmentReport(GetVrAdjustmentLogCriteria criteria, int monthsToCheck);

        List<CustomerMarginModel> GetCustomerMarginHistory(ICriteria criteria, int monthsToCheck, DateTime AdjustDate);

        IEnumerable<CustomerMarginDateModel> GetReportDate(ICriteria criteria);
    }
}