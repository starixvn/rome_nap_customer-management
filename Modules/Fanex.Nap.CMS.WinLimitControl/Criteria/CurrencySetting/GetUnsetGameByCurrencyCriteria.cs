namespace Fanex.Nap.CMS.WinLimitControl.Criteria.CurrencySetting
{
    using Fanex.Data.Repository;

    public class GetUnsetGameByCurrencyCriteria : CriteriaBase
    {
        public GetUnsetGameByCurrencyCriteria(int currencyId)
        {
            ExchangeId = currencyId;
        }

        public int ExchangeId { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_NotConfigProductSel";
        }
    }
}
