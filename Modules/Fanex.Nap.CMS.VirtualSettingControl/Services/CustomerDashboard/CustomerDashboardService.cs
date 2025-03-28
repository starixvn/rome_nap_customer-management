namespace Fanex.Nap.CMS.VirtualSettingControl.Services.CustomerDashboard
{
    using System.Linq;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.CustomerDashboard;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.CustomerDashboard;

    /// <summary>
    /// Customer Dashboard Service.
    /// </summary>
    public class CustomerDashboardService : ServiceBase, ICustomerDashboardService
    {
        public CustomerDashboardModel GetDashboardInfo(GetDashboardInfoCriteria criteria)
        {
            var (generalInfo, betCountByMonth, sportPerformance, betTypePerformance, eventChoice) =
                DynamicRepository.FetchMultiple<GeneralInfo, BetCountByMonth, SportPerformance, BetTypePerformance, EventChoice>(criteria);

            return new CustomerDashboardModel
            {
                DashboardGeneralInfo = generalInfo.FirstOrDefault(),
                DashboardBetCountByMonth = betCountByMonth,
                DashboardTopSports = sportPerformance,
                DashboardTopBettypes = betTypePerformance,
                DashboardTopEventChoice = eventChoice
            };
        }
    }
}