namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.VirtualRateDetails
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;

    public interface IVirtualRateDetailsRepository
    {
        IEnumerable<RateInfo> GetVirtualRateRanges();

        VirtualRateDetailsReport GetVirtualRateDetails(GetVirtualRateDetailsCriteria criteria);

        Result SaveVirtualRateDetails(SaveVirtualRateDetailsCriteria criteria);

        ValidateAssociatedAccountsResult ValidateAssociatedAccounts(ValidateAssociatedAccountsCriteria criteria);

        Result SaveAssociatedAccounts(SaveAssociatedAccountsCriteria criteria);

        VirtualRateLogReport GetLogs(GetVirtualRateLogsCriteria criteria);

        VirtualRateRealValues ViewRealValues(ViewRealValuesCriteria criteria);

        IEnumerable<VirtualRateDTO> GetVirtualRateDetails(ICriteria criteria);

        Result ExecuteNonQuery(ICriteria criteria);

        IEnumerable<GlobalAutoSuspendThresholdLogDTO> GetGlobalAutoSuspendThresholdLogs(ICriteria criteria);
    }
}
