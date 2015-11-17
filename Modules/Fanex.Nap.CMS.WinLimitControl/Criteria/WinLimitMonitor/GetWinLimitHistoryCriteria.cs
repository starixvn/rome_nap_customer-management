namespace Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor
{
    using Fanex.Data.Repository;

    public class GetWinLimitHistoryCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "custid")]
        public int Player { get; set; }

        [SpParam(SqlParamName = "productid")]
        public int Game { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_GameHistorySel";
        }
    }
}
