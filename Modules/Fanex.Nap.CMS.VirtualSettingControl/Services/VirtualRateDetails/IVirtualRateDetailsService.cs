namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualRateDetails
{
    using System;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;

    public interface IVirtualRateDetailsService
    {
        VirtualRateDetailsReport GetVirtualRateDetails(GetVirtualRateDetailsCriteria criteria);

        Result SaveVirtualRateDetails(SaveVirtualRateDetailsCriteria criteria);

        ValidateAssociatedAccountsResult ValidateAssociatedAccounts(ValidateAssociatedAccountsCriteria criteria);

        Result SaveAssociatedAccounts(SaveAssociatedAccountsCriteria criteria);

        VirtualRateLogReport GetLogs(GetVirtualRateLogsCriteria criteria);

        VirtualRateRealValues ViewRealValues(ViewRealValuesCriteria criteria);

        AutoSuspendLogReport GetAutoSuspendLogs(GetAutoSuspendLogsCriteria criteria);

        GlobalAutoSuspendThreshold GetGlobalAutoSuspendThreshold();

        Result UpdateGlobalAutoSuspendThreshold(VirtualRateThresholdByGroup[] sports, int userId, string ip);

        GlobalAutoSuspendThreshold GetGlobalAutoSuspendThresholdLog(int sportType, DateTime fromDate, DateTime toDate);
    }
}
