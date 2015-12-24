namespace Fanex.Nap.CMS.WinLimitControl.Criteria.CurrencySetting
{
    using Fanex.Data.Repository;

    public class UpdateRelatedPlayerWinLimitCriteria : AddCurrencyWinLimitCriteria
    {
        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CustSettingRelatedToCurrencySettingUpd";
        }
    }
}
