namespace Fanex.Nap.CMS.WinLimitControl.Criteria.CurrencySetting
{
    using Fanex.Data.Repository;

    public class UpdateCurrencyWinLimitCriteria : AddCurrencyWinLimitCriteria
    {
        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CurrencySettingUpd";
        }
    }
}
