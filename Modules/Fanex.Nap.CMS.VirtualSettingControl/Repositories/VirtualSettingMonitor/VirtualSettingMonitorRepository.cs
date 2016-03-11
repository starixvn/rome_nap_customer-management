namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.VirtualSettingMonitor
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using Fanex.Nap.CMS.VirtualSettingControl.ViewModels.VirtualSettingMonitor;
    using System.Collections.Generic;

    public class VirtualSettingMonitorRepository : IVirtualSettingMonitorRepository
    {
        public IList<VirtualSettingViewModel> GetVirtualSettingMonitor(ICriteria criteria)
        {
            return null;
        }

        public CustomerVirtualSettingModel GetCustomerVirtualSettingHistories(ICriteria criteria)
        {
            return null;
        }
    }
}
