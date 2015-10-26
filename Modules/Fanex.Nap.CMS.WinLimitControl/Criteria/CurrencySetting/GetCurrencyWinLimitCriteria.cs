namespace Fanex.Nap.CMS.WinLimitControl.Criteria.CurrencySetting
{
    using Fanex.Data.Repository;

    public class GetCurrencyWinLimitCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ProductId")]
        public int Game { get; set; }

        [SpParam(SqlParamName = "ExchangeId")]
        public int Currency { get; set; }

        [SpParam(Ignored = true)]
        public string SortColumnName { get; set; }

        [SpParam(Ignored = true)]
        public string SortDirection { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CurrencySettingSel";
        }
    }
}
