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
                return NapSettings.Settings.Environment.Equals("LIVE", StringComparison.OrdinalIgnoreCase);
            }
        }

        [SpParam(Ignored = true)]
        public int Status { get; set; }

        [SpParam(Ignored = true)]
        public bool IsBlocked
        {
            get
            {
                return Status == 1;
            }
        }

        public override string GetSettingKey()
        {
            if (IsBlocked)
            {
                return "NAP_WinLimit_BlockedMemberSel";    
            }

            return "NAP_WinLimit_UnblockedMemberSel";
        }
    }
}
