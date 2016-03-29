namespace Fanex.Nap.CMS.VirtualSettingControl.Services.CustomerDashboard
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.CustomerDashboard;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.CustomerDashboard;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Customer Dashboard Service.
    /// </summary>
    public class CustomerDashboardService : ServiceBase, ICustomerDashboardService
    {
        public CustomerDashboardModel GetDashboardInfo(ICriteria criteria)
        {
            return null;
        }

        /*
        /// <summary>
        /// Gets the general information.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <returns>
        /// General info list.
        /// </returns>
        public IEnumerable<GeneralInfo> GetGeneralInfo(int month)
        {
            IEnumerable<GeneralInfo> result = null;

            var summaryCriteria = new GetGeneralInfoCriteria
            {
                ReportType = 2,
                FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1 - month),
                ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
            };
            result = this.GetData<GeneralInfo>(summaryCriteria);

            return result;
        }

        /// <summary>
        /// Gets the top win league.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <returns>
        /// List of league.
        /// </returns>
        public IEnumerable<League> GetTopWinLeague(int month)
        {
            var topLeagueCriteria = new GetTopWinLeagueCriteria
            {
                ReportType = 2,
                FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1 - month),
                ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
            };

            IEnumerable<League> result = this.GetData<League>(topLeagueCriteria);

            result = result.Take(5);

            return result;
        }

        /// <summary>
        /// Gets the top trader.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <returns>
        /// List Top Trader.
        /// </returns>
        public IEnumerable<EventChoice> GetTopTrader(int month)
        {
            var topTraderCriteria = new GetTopTraderCriteria
            {
                ReportType = 2,
                FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1 - month),
                ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)),
                Top = 5,
                IsBottom = 0
            };

            IEnumerable<EventChoice> result = this.GetData<EventChoice>(topTraderCriteria);

            return result;
        }

        /// <summary>
        /// Gets the top bet type.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <returns>
        /// List Bet type.
        /// </returns>
        public IEnumerable<BetTypePerformance> GetTopBettype(int month)
        {
            var topBettypeCriteria = new GetBetTypePerformanceCriteria
            {
                ReportType = 2,
                FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1 - month),
                ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
            };

            IEnumerable<BetTypePerformance> result = this.GetData<BetTypePerformance>(topBettypeCriteria);

            return result.Take(10);
        }

        /// <summary>
        /// Gets the summary data.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <returns>
        /// List Month Items.
        /// </returns>
        public IEnumerable<MonthAmountItem> GetSummaryData(int month)
        {
            var summaryCriteria = new GetSummaryInfoCriteria
            {
                ReportType = 2,
                FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1 - month),
                ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
            };

            List<BetCountByMonth> rawData = this.GetData<BetCountByMonth>(summaryCriteria).ToList();
            rawData = rawData.OrderBy(x => x.EventDate).ToList();

            var result = new List<MonthAmountItem>();
            foreach (var item in rawData)
            {
                var monthItem = new MonthAmountItem(item);
                result.Add(monthItem);
            }

            return result;
        }

        /// <summary>
        /// Gets the team data.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <returns>List of team performance.</returns>
        public IEnumerable<TeamPerformance> GetTeamData(int month)
        {
            var teamCriteria = new GetTeamPerformanceCriteria
            {
                ReportType = 2,
                FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1 - month),
                ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
            };

            IEnumerable<TeamPerformance> teamData = this.GetData<TeamPerformance>(teamCriteria);

            return teamData;
        }
        */
    }
}