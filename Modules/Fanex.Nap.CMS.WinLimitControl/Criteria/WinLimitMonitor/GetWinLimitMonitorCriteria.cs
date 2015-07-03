namespace Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor
{
    using Fanex.Data.Repository;

    public class GetWinLimitMonitorCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ExchangeId")]
        public int Currency { get; set; }

        [SpParam(SqlParamName = "ProductId")]
        public int Game { get; set; }

        public string Username { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRow { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimitControl_WinLimitMonitorSel";
        }
    }
}
