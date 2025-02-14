namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.CustomerDashboard
{
    using Fanex.Data;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.CustomerDashboard;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.CustomerDashboard;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class CustomerDashboardRepository : ICustomerDashboardRepository
    {
        private const string TurnOver = "turnover";
        private const string Winloss = "winloss";
        private const string Margin = "margin";
        private const string BetCount = "betcount";

        public CustomerDashboardModel GetDashboardInfo(GetDashboardInfoCriteria criteria)
        {
            using (var objDb = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                var dataSet = objDb.ExecuteDataSet(criteria);

                return ParseDashboardData(dataSet);
            }
        }

        public CustomerDashboardModel ParseDashboardData(DataSet dataSet)
        {
            var customerDashboardModel = new CustomerDashboardModel
            {
                DashboardGeneralInfo = ParseGeneralInfo(dataSet.Tables[0]),
                DashboardBetCountByMonth = ParseBetCount(dataSet.Tables[1]),
                DashboardTopSports = ParseSportPerformance(dataSet.Tables[2]),
                DashboardTopBettypes = ParseBetTypePerformance(dataSet.Tables[3]),
                DashboardTopEventChoice = ParseEventChoice(dataSet.Tables[4])
            };

            return customerDashboardModel;
        }

        #region Utilities

        public GeneralInfo ParseGeneralInfo(DataTable data)
        {
            var generalInfo = new GeneralInfo
            {
                Turnover = Convert.ToDecimal(data.Rows[0][TurnOver].ToString()),
                Winloss = Convert.ToDecimal(data.Rows[0][Winloss].ToString()),
                Margin = Convert.ToDecimal(data.Rows[0][Margin].ToString()),
                BetCount = Convert.ToInt32(data.Rows[0][BetCount].ToString())
            };

            return generalInfo;
        }

        public IEnumerable<BetCountByMonth> ParseBetCount(DataTable data)
        {
            var betcounts = new List<BetCountByMonth>();
            foreach (DataRow dataRow in data.Rows)
            {
                betcounts.Add(new BetCountByMonth
                {
                    ReportYear = Convert.ToInt16(dataRow["reportyear"].ToString()),
                    ReportMonth = Convert.ToInt16(dataRow["reportmonth"].ToString()),
                    BetCount = Convert.ToInt16(dataRow[BetCount].ToString())
                });
            }

            return betcounts;
        }

        public IEnumerable<SportPerformance> ParseSportPerformance(DataTable data)
        {
            var topSports = new List<SportPerformance>();
            foreach (DataRow dataRow in data.Rows)
            {
                topSports.Add(new SportPerformance
                {
                    SportType = Convert.ToInt16(dataRow["sporttype"].ToString()),
                    SportName = dataRow["sportname"].ToString(),
                    Turnover = Convert.ToDecimal(dataRow[TurnOver].ToString()),
                    Winloss = Convert.ToDecimal(dataRow[Winloss].ToString()),
                    Margin = Convert.ToDecimal(dataRow[Margin].ToString())
                });
            }

            return topSports;
        }

        public IEnumerable<BetTypePerformance> ParseBetTypePerformance(DataTable data)
        {
            var topBettypes = new List<BetTypePerformance>();
            foreach (DataRow dataRow in data.Rows)
            {
                topBettypes.Add(new BetTypePerformance
                {
                    BetType = Convert.ToInt16(dataRow["bettype"].ToString()),
                    BetTypeName = dataRow["bettypename"].ToString(),
                    Turnover = Convert.ToDecimal(dataRow[TurnOver].ToString()),
                    Winloss = Convert.ToDecimal(dataRow[Winloss].ToString()),
                    Margin = Convert.ToDecimal(dataRow[Margin].ToString())
                });
            }

            return topBettypes;
        }

        public IEnumerable<EventChoice> ParseEventChoice(DataTable data)
        {
            var topEventChoices = new List<EventChoice>();
            foreach (DataRow dataRow in data.Rows)
            {
                topEventChoices.Add(new EventChoice
                {
                    IsLive = Convert.ToBoolean(dataRow["islive"].ToString()),
                    Turnover = Convert.ToDecimal(dataRow[TurnOver].ToString()),
                    Winloss = Convert.ToDecimal(dataRow[Winloss].ToString()),
                    Margin = Convert.ToDecimal(dataRow[Margin].ToString())
                });
            }

            return topEventChoices;
        }

        #endregion
    }
}
