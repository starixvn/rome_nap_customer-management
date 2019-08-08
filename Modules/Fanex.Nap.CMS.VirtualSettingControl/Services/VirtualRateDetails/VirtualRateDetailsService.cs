namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualRateDetails
{
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Repositories.VirtualRateDetails;

    public class VirtualRateDetailsService : IVirtualRateDetailsService
    {
        private readonly IVirtualRateDetailsRepository _repository;

        public VirtualRateDetailsService(IVirtualRateDetailsRepository repository = null)
        {
            _repository = repository ?? new VirtualRateDetailsRepository();
        }

        public VirtualRateDetailsReport GetVirtualRateDetails(GetVirtualRateDetailsCriteria criteria)
            => _repository.GetVirtualRateDetails(criteria);

        public Result SaveVirtualRateDetails(SaveVirtualRateDetailsCriteria criteria)
            => _repository.SaveVirtualRateDetails(criteria);

        public ValidateAssociatedAccountsResult ValidateAssociatedAccounts(ValidateAssociatedAccountsCriteria criteria)
            => _repository.ValidateAssociatedAccounts(criteria);

        public Result SaveAssociatedAccounts(SaveAssociatedAccountsCriteria criteria)
            => _repository.SaveAssociatedAccounts(criteria);

        public VirtualRateLogReport GetLogs(GetVirtualRateLogsCriteria criteria)
            => _repository.GetLogs(criteria);

        public VirtualRateRealValues ViewRealValues(ViewRealValuesCriteria criteria)
            => _repository.ViewRealValues(criteria);
    }
}
