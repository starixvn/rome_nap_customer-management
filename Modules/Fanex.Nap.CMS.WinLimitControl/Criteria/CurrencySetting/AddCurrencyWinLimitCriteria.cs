namespace Fanex.Nap.CMS.WinLimitControl.Criteria.CurrencySetting
{
    using Fanex.Data.Repository;

    public class AddCurrencyWinLimitCriteria : CriteriaBase
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

        public int UserId { get; set; }

        public string IP { get; set; }

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
