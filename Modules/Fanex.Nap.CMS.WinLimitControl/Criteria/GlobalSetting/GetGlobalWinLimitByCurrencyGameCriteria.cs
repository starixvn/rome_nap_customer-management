namespace Fanex.Nap.CMS.WinLimitControl.Criteria.GlobalSetting
{
    using Fanex.Data.Repository;

    public class GetGlobalWinLimitByCurrencyGameCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ExchangeId")]
        public int Currency { get; set; }

        [SpParam(SqlParamName = "ProductId")]
        public int Game { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CurrencySettingSel";
        }
    }
}
