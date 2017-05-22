namespace Fanex.Nap.CMS.VirtualSettingControl.Services.CustomerDashboard
{
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.CustomerDashboard;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.CustomerDashboard;
    using Fanex.Nap.CMS.VirtualSettingControl.Repositories.CustomerDashboard;

    /// <summary>
    /// Customer Dashboard Service.
    /// </summary>
    public class CustomerDashboardService : ServiceBase, ICustomerDashboardService
    {
        private readonly CustomerDashboardRepository _dashboardRepository;

        public CustomerDashboardService(CustomerDashboardRepository dashboardRepository = null)
        {
            _dashboardRepository = dashboardRepository ?? new CustomerDashboardRepository();
        }

        public CustomerDashboardModel GetDashboardInfo(GetDashboardInfoCriteria criteria)
        {
            var dashboardModel = _dashboardRepository.GetDashboardInfo(criteria);

            return dashboardModel;
        }
    }
}