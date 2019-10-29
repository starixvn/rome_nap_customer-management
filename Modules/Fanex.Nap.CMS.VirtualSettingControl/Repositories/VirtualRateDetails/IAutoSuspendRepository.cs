namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.VirtualRateDetails
{
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;

    public interface IAutoSuspendRepository
    {
        AutoSuspendLogReport GetAutoSuspendLogs(GetAutoSuspendLogsCriteria criteria);
    }
}
