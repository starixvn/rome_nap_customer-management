namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.VirtualSettingMonitor
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.ViewModels.VirtualSettingMonitor;
    using System.Collections.Generic;

    public interface IVirtualSettingMonitorRepository
    {
        IList<VirtualSettingViewModel> GetVirtualSettingMonitor(ICriteria criteria);

        CustomerVirtualSettingModel GetCustomerVirtualSettingHistories(ICriteria criteria);
    }
}
