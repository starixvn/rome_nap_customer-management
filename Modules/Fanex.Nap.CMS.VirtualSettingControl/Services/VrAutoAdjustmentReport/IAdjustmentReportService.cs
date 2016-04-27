using System.Linq;
using Fanex.NAP.Core.Domain;

namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VrAutoAdjustmentReport
{
    using System;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Utility;
    using Models.VrAutoAdjustmentReport;

    public interface IAdjustmentReportService
    {
        QueryResult<VrAutoAdjustmentLogEntry> GetReport(PagingCriteria criteria);

        ILookup<int, MarginModel> GetVirtualSettingLatestMonths(IEnumerable<int> custIds, DateTime reportDate, int monthsToCheck);
    }
}