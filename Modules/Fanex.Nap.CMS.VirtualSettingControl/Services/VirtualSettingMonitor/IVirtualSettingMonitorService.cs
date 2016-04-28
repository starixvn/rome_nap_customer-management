namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualSettingMonitor
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.ViewModels.VirtualSettingMonitor;
    using System.Collections.Generic;

    public interface IVirtualSettingMonitorService
    {
        VirtualSettingViewModel GetVirtualSettingMonitor(GetVirtualSettingMonitorCriteria criteria, int[] monthList);

        IEnumerable<MarginModel> GetCustomerVirtualSettingHistories(ICriteria criteria, int[] monthList);

        IEnumerable<MarginModel> GetCustomerVirtualSettingLatestMonths(ICriteria criteria, int[] monthList);
    }
}
