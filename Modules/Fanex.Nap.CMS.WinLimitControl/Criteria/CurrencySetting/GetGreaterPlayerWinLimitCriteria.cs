namespace Fanex.Nap.CMS.WinLimitControl.Criteria.CurrencySetting
{
    using Fanex.Data.Repository;

    public class GetGreaterPlayerWinLimitCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "ExchangeID")]
        public int CurrencyId { get; set; }

        [SpParam(SqlParamName = "ProductID")]
        public int GameID { get; set; }

        public decimal MaxWinLimit { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_GreaterWinLimitPlayerSel";
        }
    }
}
