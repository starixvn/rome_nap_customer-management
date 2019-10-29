namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualRateDetails
{
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Repositories.VirtualRateDetails;

    public class VirtualRateDetailsService : IVirtualRateDetailsService
    {
        private readonly IVirtualRateDetailsRepository _repository;
        private readonly IAutoSuspendRepository _autoSuspendRepository;

        public VirtualRateDetailsService(
            IVirtualRateDetailsRepository repository = null,
            IAutoSuspendRepository autoSuspendRepository = null)
        {
            _repository = repository ?? new VirtualRateDetailsRepository();
            _autoSuspendRepository = autoSuspendRepository ?? new AutoSuspendRepository();
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

        public AutoSuspendLogReport GetAutoSuspendLogs(GetAutoSuspendLogsCriteria criteria)
            => _autoSuspendRepository.GetAutoSuspendLogs(criteria);
    }
}
