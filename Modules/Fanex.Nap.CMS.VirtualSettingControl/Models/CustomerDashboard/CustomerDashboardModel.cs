namespace Fanex.Nap.CMS.VirtualSettingControl.Models.CustomerDashboard
{
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualSettingMonitor;
    using System.Collections.Generic;

    /// <summary>
    /// Monthly amount item.
    /// </summary>
    public class CustomerDashboardModel
    {
        public CustomerDashboardModel()
        {
            DashboardMarginModels = new List<MarginModel>();
            DashboardBetCountByMonth = new List<BetCountByMonth>();
            DashboardTopSports = new List<SportPerformance>();
            DashboardTopBettypes = new List<BetTypePerformance>();
            DashboardTopEventChoice = new List<EventChoice>();
        }

        public IEnumerable<MarginModel> DashboardMarginModels { get; set; }

        public GeneralInfo DashboardGeneralInfo { get; set; }

        public IEnumerable<BetCountByMonth> DashboardBetCountByMonth { get; set; }

        public IEnumerable<SportPerformance> DashboardTopSports { get; set; }

        public IEnumerable<BetTypePerformance> DashboardTopBettypes { get; set; }

        public IEnumerable<EventChoice> DashboardTopEventChoice { get; set; }
    }
}
