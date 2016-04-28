namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VrAutoAdjustmentReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Utility;
    using Fanex.NAP.CMS.Criteria.Common;
    using Models.VrAutoAdjustmentReport;

    public interface IAdjustmentReportService
    {
        PagingQueryResult<VrAutoAdjustmentLogEntry> GetReport(PagingCriteria criteria);

        ILookup<int, MarginModel> GetVirtualSettingLatestMonths(IEnumerable<int> custIds, DateTime reportDate, int monthsToCheck);
    }
}