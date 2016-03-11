namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualSettingMonitor
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.ViewModels.VirtualSettingMonitor;
    using System.Collections.Generic;

    public interface IVirtualSettingMonitorService
    {
        VirtualSettingViewModel GetVirtualSettingMonitor(ICriteria criteria, int[] monthList);

        CustomerVirtualSettingModel GetCustomerVirtualSettingHistories(ICriteria criteria, int[] monthList);
    }
}
