namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualSettingMonitorAdvance
{
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualSettingMonitorAdvance;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitorAdvance;
    using System.Collections.Generic;

    public interface IVirtualSettingMonitorAdvanceService
    {
        List<CustomerVirtualSettingAdvanceModel> GetVirtualSettingAdvanceMonitor(GetVirtualSettingMonitorAdvanceCriteria criteria, int monthsToCheck);

        void ExportVirtualSettingAdvanceMonitor(GetVirtualSettingMonitorAdvanceCriteria criteria, int monthsToCheck);
    }
}
