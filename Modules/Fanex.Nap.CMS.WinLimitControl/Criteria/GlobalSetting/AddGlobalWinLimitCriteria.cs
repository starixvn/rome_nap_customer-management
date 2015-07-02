namespace Fanex.Nap.CMS.WinLimitControl.Criteria.GlobalSetting
{
    using Fanex.Data.Repository;

    public class AddGlobalWinLimitCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ExchangeID")]
        public int CurrencyId { get; set; }

        [SpParam(SqlParamName = "ProductID")]
        public int GameId { get; set; }

        public decimal MaxWinLimit { get; set; }

        [SpParam(SqlParamName = "ErrCode")]
        public int ErrorCode { get; set; }

        [SpParam(SqlParamName = "ErrMsg")]
        public string ErrorMessage { get; set; }

        [SpParam(Ignored = true)]
        public string CurrencyName { get; set; }

        [SpParam(Ignored = true)]
        public string GameName { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CurrencySettingIns";
        }
    }
}
