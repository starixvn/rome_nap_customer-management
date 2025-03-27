using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails;
using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;

namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualRateDetails
{
    public interface IAutoSuspendService
    {
        AutoSuspendLogReport GetAutoSuspendLogs(GetAutoSuspendLogsCriteria criteria);
    }
}
