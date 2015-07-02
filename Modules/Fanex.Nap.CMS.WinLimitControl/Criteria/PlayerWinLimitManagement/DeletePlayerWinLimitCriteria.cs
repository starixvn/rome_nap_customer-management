namespace Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerWinLimitManagement
{
    using Fanex.Data.Repository;

    public class DeletePlayerWinLimitCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "CustId")]
        public int Player { get; set; }

        [SpParam(SqlParamName = "ProductId")]
        public int Game { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CustSettingDel";
        }
    }
}
