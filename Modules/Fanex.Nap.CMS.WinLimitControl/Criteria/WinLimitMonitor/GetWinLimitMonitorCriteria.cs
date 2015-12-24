namespace Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor
{
    using System;
    using Fanex.Data.Repository;
    using Fanex.NAP.Core.Configuration;

    public class GetWinLimitMonitorCriteria : CriteriaBase
    {
        public string Username { get; set; }

        [SpParam(SqlParamName = "ExchangeID")]
        public int Currency { get; set; }

        [SpParam(SqlParamName = "ProductID")]
        public int Game { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRow { get; set; }

        public bool ExcludeTestData
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ExcludeTestData"].Equals("1");
            }
        }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_GameSel"; 
        }
    }
}
