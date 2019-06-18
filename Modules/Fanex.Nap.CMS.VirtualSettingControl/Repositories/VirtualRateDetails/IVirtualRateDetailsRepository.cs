namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.VirtualRateDetails
{
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;

    public interface IVirtualRateDetailsRepository
    {
        VirtualRateDetailsReport GetVirtualRateDetails(GetVirtualRateDetailsCriteria criteria);

        Result SaveVirtualRateDetails(SaveVirtualRateDetailsCriteria criteria);

        ValidateAssociatedAccountsResult ValidateAssociatedAccounts(ValidateAssociatedAccountsCriteria criteria);

        Result SaveAssociatedAccounts(SaveAssociatedAccountsCriteria criteria);

        VirtualRateLogReport GetLogs(GetVirtualRateLogsCriteria criteria);

        VirtualRateRealValues ViewRealValues(ViewRealValuesCriteria criteria);
    }
}
