namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualRateDetails
{
    using System;
    using System.Collections.Generic;
    using Criteria.VirtualRateDetails;
    using Infrastructure.Models;
    using Models.VirtualRateDetails;

    public interface IVirtualRateDetailsService
    {
        VirtualRateDetailsReport GetVirtualRateDetails(GetVirtualRateDetailsCriteria criteria);

        Result SaveVirtualRateDetails(SaveVirtualRateDetailsCriteria criteria);

        ValidateAssociatedAccountsResult ValidateAssociatedAccounts(ValidateAssociatedAccountsCriteria criteria);

        Result SaveAssociatedAccounts(SaveAssociatedAccountsCriteria criteria);

        VirtualRateLogReport GetLogs(GetVirtualRateLogsCriteria criteria);

        VirtualRateRealValues ViewRealValues(ViewRealValuesCriteria criteria);

        AutoSuspendLogReport GetAutoSuspendLogs(GetAutoSuspendLogsCriteria criteria);

        GlobalAutoSuspendThreshold GetGlobalAutoSuspendThreshold(int sportType);

        Result UpdateGlobalAutoSuspendThreshold(int sportType, VirtualRateThresholdByGroup[] sports, int userId, string ip);

        IEnumerable<GlobalAutoSuspendThresholdLog> GetGlobalAutoSuspendThresholdLogs(int sportType, DateTime fromDate, DateTime toDate);

        IEnumerable<BetTypeVirtualRateMapping> GetBetTypeVirtualRateMapping(int sportType);
    }
}
