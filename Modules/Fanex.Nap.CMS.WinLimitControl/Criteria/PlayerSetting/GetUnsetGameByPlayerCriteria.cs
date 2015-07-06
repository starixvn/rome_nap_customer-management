namespace Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerSetting
{
    using Fanex.Data.Repository;

    public class GetUnsetGameByPlayerCriteria : CriteriaBase
    {
        public string Username { get; set; }

        [SpParam(SqlParamName = "ErrCode")]
        public int ErrorCode { get; set; }

        [SpParam(SqlParamName = "ErrMsg")]
        public string ErrorMessage { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CustNotConfigProductSel";
        }
    }
}
