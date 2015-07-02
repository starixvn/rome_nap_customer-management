namespace Fanex.Nap.CMS.WinLimitControl.Criteria.GlobalSetting
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
