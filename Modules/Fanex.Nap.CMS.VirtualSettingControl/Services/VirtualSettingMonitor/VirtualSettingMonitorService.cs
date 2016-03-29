namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualSettingMonitor
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.ViewModels.VirtualSettingMonitor;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class VirtualSettingMonitorService : ServiceBase, IVirtualSettingMonitorService
    {
        public VirtualSettingMonitorService()
        {
        }

        public VirtualSettingViewModel GetVirtualSettingMonitor(ICriteria criteria, int[] monthList)
        {
            var rawData = GetData<VirtualSettingModel>(criteria).ToList();
            return BindVirtualSettingData(rawData, monthList);
        }

        public CustomerVirtualSettingModel GetCustomerVirtualSettingHistories(ICriteria criteria, int[] monthList)
        {
            var rawData = GetData<VirtualSettingModel>(criteria).ToList();
            return BindCustomerVirtualSettingHistories(rawData, monthList);
        }

        #region Utilities

        public VirtualSettingViewModel BindVirtualSettingData(List<VirtualSettingModel> rawData, int[] monthList)
        {
            var virtualSettingViewModel = new VirtualSettingViewModel();
            var virtualSettingCategorized = rawData.GroupBy(l => l.CustId);

            foreach (var cust in virtualSettingCategorized)
            {
                var newCust = new CustomerVirtualSettingModel
                    {
                        CustId = cust.Key,
                        Username = cust.First().Username,
                        VRate = cust.First().VRate
                    };

                var virtualSetting = cust.Select(l => l).ToList();
                foreach(var month in monthList)
                {
                    var custSetting = FindVirtualSetting(virtualSetting, month);
                    newCust.CustMargin.Add( custSetting != null
                            ? new MarginModel(custSetting.ReportMonth, custSetting.ReportYear, custSetting.Turnover, custSetting.Winloss)
                            : new MarginModel(month, 0));
                }

                // The Average Performance record
                var sumTurnover = newCust.CustMargin.Sum( l => l.Turnover);
                var sumWinloss = newCust.CustMargin.Sum( l => l.Winloss);
                newCust.CustMargin.Add(new MarginModel(0, 0, sumTurnover, sumWinloss));

                virtualSettingViewModel.CustMargin.Add(newCust);
            }

            return virtualSettingViewModel;
        }

        public CustomerVirtualSettingModel BindCustomerVirtualSettingHistories(List<VirtualSettingModel> rawData, int[] monthList)
        {
            var custVirtualSettingHistory = new CustomerVirtualSettingModel();
            foreach (var month in monthList)
            {
                var custSetting = FindVirtualSetting(rawData, month);
                custVirtualSettingHistory.CustMargin.Add(custSetting != null
                        ? new MarginModel(custSetting.ReportMonth, custSetting.ReportYear, custSetting.Turnover, custSetting.Winloss)
                        : new MarginModel(month, 0));
            }

            // The Average Performance record
            var sumTurnover = rawData.Sum(l => l.Turnover);
            var sumWinloss = rawData.Sum(l => l.Winloss);
            custVirtualSettingHistory.CustMargin.Add(new MarginModel(0, 0, sumTurnover, sumWinloss));

            return custVirtualSettingHistory;
        }

        public VirtualSettingModel FindVirtualSetting(List<VirtualSettingModel> rawData, int month)
        {
            return rawData.Any(l => l.ReportMonth == month)
                ? rawData.First(l => l.ReportMonth == month)
                : null;
        }

        public CustomerVirtualSettingModel BindVirtualSettingData(VirtualSettingModel rawData)
        {
            return null;
        }
        #endregion
    }
}
