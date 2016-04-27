using System.Linq;

namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VrAutoAdjustmentReport
{
    using System;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VrAutoAdjustmentReport;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Utility;
    using Models.VrAutoAdjustmentReport;

    public class AdjustmentReportService : ServiceBase, IAdjustmentReportService
    {
        public QueryResult<VrAutoAdjustmentLogEntry> GetReport(PagingCriteria criteria)
        {
            var result = this.GetData<VrAutoAdjustmentLogEntry>(criteria);

            return new QueryResult<VrAutoAdjustmentLogEntry>(criteria, result);
        }

        public ILookup<int, MarginModel> GetVirtualSettingLatestMonths(IEnumerable<int> custIds, DateTime reportDate, int monthsToCheck)
        {
            var criteria = new GetCustomerMarginByMonthCriteria
            {
                CustIds = string.Join(", ", custIds),
                MonthToCheck = monthsToCheck,
                ReportToDate = reportDate
            };

            return GetData<VirtualSettingModel>(criteria).ToLookup(m => m.CustId, m => new MarginModel(m.ReportMonth, m.ReportYear, m.Turnover, m.Winloss));
        }
    }
}