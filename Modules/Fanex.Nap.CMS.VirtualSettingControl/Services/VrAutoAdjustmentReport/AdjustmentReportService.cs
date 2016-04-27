namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VrAutoAdjustmentReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Repository;
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

        public IEnumerable<MarginModel> GetVirtualSettingLatestMonths(int custId, DateTime reportDate, int monthsToCheck)
        {
            var dataByMonths = new List<VirtualSettingModel>();
            var monthList = Helper.GenerateMonthList(reportDate, monthsToCheck);

            for (var i = 0; i < monthsToCheck; i++)
            {
                var monthYear = reportDate.AddMonths(-i);

                var criteria = new GetCustomerMarginByMonthCriteria
                {
                    CustId = custId,
                    Month = monthYear.Month,
                    Year = monthYear.Year
                };

                var result = GetData<VirtualSettingModel>(criteria).ToList();

                dataByMonths.AddRange(result);
            }

            var refinedData = BindCustomerVirtualSettingHistories(dataByMonths, monthList);

            return refinedData.CustMargin;
        }

        private CustomerVirtualSettingModel BindCustomerVirtualSettingHistories(List<VirtualSettingModel> rawData, int[] monthList)
        {
            var custVirtualSettingHistory = new CustomerVirtualSettingModel();
            var currentYear = DateTime.Now.Year;
            foreach (var month in monthList)
            {
                if (month == 12)
                {
                    currentYear = currentYear - 1;
                }

                var custSetting = FindVirtualSetting(rawData, month);
                custVirtualSettingHistory.CustMargin.Add(custSetting != null
                        ? new MarginModel(custSetting.ReportMonth, custSetting.ReportYear, custSetting.Turnover, custSetting.Winloss)
                        : new MarginModel(month, currentYear));
            }

            // The Average Performance record
            var sumTurnover = rawData.Sum(l => l.Turnover);
            var sumWinloss = rawData.Sum(l => l.Winloss);
            custVirtualSettingHistory.CustMargin.Add(new MarginModel(0, 0, sumTurnover, sumWinloss));

            return custVirtualSettingHistory;
        }

        private VirtualSettingModel FindVirtualSetting(List<VirtualSettingModel> rawData, int month)
        {
            return rawData.FirstOrDefault(l => l.ReportMonth == month);
        }
    }
}