namespace Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerSetting
{
    using Fanex.Data.Repository;

    public class DeletePlayerWinLimitCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "CustId")]
        public int Player { get; set; }

        [SpParam(SqlParamName = "ProductId")]
        public int Game { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CustSettingDel";
        }
    }
}
