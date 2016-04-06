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
        public CustomerDashboardRepository()
        {
        }

        public CustomerDashboardModel GetDashboardInfo(GetDashboardInfoCriteria criteria)
        {
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var dataSet = objDb.ExecuteDataSet();

                return ParseDashboardData(dataSet);
            }
        }

        public CustomerDashboardModel ParseDashboardData(DataSet dataSet)
        {
            var customerDashboardModel = new CustomerDashboardModel();
            customerDashboardModel.DashboardGeneralInfo = ParseGeneralInfo(dataSet.Tables[0]);
            customerDashboardModel.DashboardBetCountByMonth = ParseBetCount(dataSet.Tables[1]);
            customerDashboardModel.DashboardTopSports = ParseSportPerformance(dataSet.Tables[2]);
            customerDashboardModel.DashboardTopBettypes = ParseBetTypePerformance(dataSet.Tables[3]);
            customerDashboardModel.DashboardTopEventChoice = ParseEventChoice(dataSet.Tables[4]);

            return customerDashboardModel;
        }

        #region Utilities

        public GeneralInfo ParseGeneralInfo(DataTable data)
        {
            var generalInfo = new GeneralInfo
            {
                Turnover = Convert.ToDecimal(data.Rows[0]["turnover"].ToString()),
                Winloss = Convert.ToDecimal(data.Rows[0]["winloss"].ToString()),
                Margin = Convert.ToDecimal(data.Rows[0]["margin"].ToString()),
                BetCount = Convert.ToInt32(data.Rows[0]["betcount"].ToString())

            };

            return generalInfo;
        }

        public IEnumerable<BetCountByMonth> ParseBetCount(DataTable data)
        {
            var betcounts = new List<BetCountByMonth>();
            foreach(DataRow dataRow in data.Rows)
            {
                betcounts.Add(new BetCountByMonth
                {
                    ReportYear = Convert.ToInt16(dataRow["reportyear"].ToString()),
                    ReportMonth = Convert.ToInt16(dataRow["reportmonth"].ToString()),
                    BetCount = Convert.ToInt16(dataRow["betcount"].ToString())
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
                    Turnover = Convert.ToDecimal(dataRow["turnover"].ToString()),
                    Winloss = Convert.ToDecimal(dataRow["winloss"].ToString()),
                    Margin = Convert.ToDecimal(dataRow["margin"].ToString())
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
                    Turnover = Convert.ToDecimal(dataRow["turnover"].ToString()),
                    Winloss = Convert.ToDecimal(dataRow["winloss"].ToString()),
                    Margin = Convert.ToDecimal(dataRow["margin"].ToString())
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
                    Turnover = Convert.ToDecimal(dataRow["turnover"].ToString()),
                    Winloss = Convert.ToDecimal(dataRow["winloss"].ToString()),
                    Margin = Convert.ToDecimal(dataRow["margin"].ToString())
                });
            }

            return topEventChoices;
        }

        #endregion
    }
}
