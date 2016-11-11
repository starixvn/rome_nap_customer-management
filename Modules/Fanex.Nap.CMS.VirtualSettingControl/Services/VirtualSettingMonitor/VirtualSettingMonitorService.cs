namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualSettingMonitor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.ViewModels.VirtualSettingMonitor;

    public class VirtualSettingMonitorService : ServiceBase, IVirtualSettingMonitorService
    {
        public VirtualSettingMonitorService()
        {
        }

        public VirtualSettingViewModel GetVirtualSettingMonitor(GetVirtualSettingMonitorCriteria criteria, int[] monthList)
        {
            var custInfo = GetData<VirtualSettingModel>(criteria).ToList();

            var custMarginCriteria = new GetCustomerMarginHistoryCriteria
            {
                CustIds = string.Join(",", custInfo.Select(l => l.CustId)),
                MonthToCheck = 12, //criteria.MonthToCheck,
                SportId = criteria.SportId,
                ReportToDate = criteria.ReportToDate
            };

            var custMargin = GetData<VirtualSettingModel>(custMarginCriteria).ToList();

            return BindVirtualSettingData(custInfo, custMargin, monthList);
        }

        public IEnumerable<MarginModel> GetCustomerVirtualSettingHistories(ICriteria criteria, int[] monthList)
        {
            var rawData = GetData<VirtualSettingModel>(criteria).ToList();
            return BindCustomerVirtualSettingHistories(rawData, monthList);
        }

        #region Utilities

        public VirtualSettingViewModel BindVirtualSettingData(List<VirtualSettingModel> custInfo, List<VirtualSettingModel> custMargin, int[] monthList)
        {
            var virtualSettingViewModel = new VirtualSettingViewModel();
            var custMarginCategorized = custMargin.GroupBy(l => l.CustId);

            foreach (var cust in custInfo)
            {
                var listCustMargin = custMarginCategorized.Any(l => l.Key == cust.CustId)
                    ? custMarginCategorized.Single(l => l.Key == cust.CustId).Select(l => l).ToList()
                    : new List<VirtualSettingModel>();
                var newCust = new CustomerVirtualSettingModel
                {
                    CustId = cust.CustId,
                    Username = cust.Username,
                    VRate = cust.VRate
                };

                var currentYear = DateTime.Now.Year;

                foreach (var month in monthList)
                {
                    if (month == 12)
                    {
                        currentYear = currentYear - 1;
                    }

                    var custSetting = FindVirtualSetting(listCustMargin, month, currentYear);
                    newCust.CustMargin.Add(custSetting != null
                            ? new MarginModel(custSetting.ReportMonth, custSetting.ReportYear, custSetting.Turnover, custSetting.Winloss, custSetting.BetCount)
                            : new MarginModel(month, currentYear));
                }

                // The Average Performance record
                var sumTurnover = newCust.CustMargin.Sum(l => l.Turnover);
                var sumWinloss = newCust.CustMargin.Sum(l => l.Winloss);
                var sumBet = newCust.CustMargin.Sum(l => l.BetCount);
                newCust.CustMargin.Add(new MarginModel(0, 0, sumTurnover, sumWinloss, sumBet));

                if (sumWinloss > 0)
                {
                    virtualSettingViewModel.CustMargin.Add(newCust);
                }
            }

            return virtualSettingViewModel;
        }

        public IEnumerable<MarginModel> BindCustomerVirtualSettingHistories(List<VirtualSettingModel> rawData, int[] monthList)
        {
            var custVirtualSettingHistory = new List<MarginModel>();
            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            var encounterFirstDecember = true;
            foreach (var month in monthList)
            {
                if (month == 12)
                {
                    if (currentMonth == 12 && encounterFirstDecember)
                    {
                        encounterFirstDecember = false;
                    }
                    else
                    {
                        currentYear = currentYear - 1;
                    }
                }

                if (currentMonth == 12)
                {
                    currentYear = currentYear + 1;
                }

                var custSetting = FindVirtualSetting(rawData, month, currentYear);
                custVirtualSettingHistory.Add(custSetting != null
                        ? new MarginModel(custSetting.ReportMonth, custSetting.ReportYear, custSetting.Turnover, custSetting.Winloss)
                        : new MarginModel(month, currentYear));
            }

            // The Average Performance record
            var sumTurnover = rawData.Sum(l => l.Turnover);
            var sumWinloss = rawData.Sum(l => l.Winloss);
            custVirtualSettingHistory.Add(new MarginModel(0, 0, sumTurnover, sumWinloss));

            return custVirtualSettingHistory;
        }

        public VirtualSettingModel FindVirtualSetting(List<VirtualSettingModel> rawData, int month, int year)
        {
            return rawData.Any(l => l.ReportMonth == month && l.ReportYear == year)
                ? rawData.First(l => l.ReportMonth == month && l.ReportYear == year)
                : null;
        }

        #endregion Utilities
    }
}