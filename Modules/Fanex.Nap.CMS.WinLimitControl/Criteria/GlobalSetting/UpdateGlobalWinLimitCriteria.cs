namespace Fanex.Nap.CMS.WinLimitControl.Criteria.GlobalSetting
{
    using Fanex.Data.Repository;

    public class UpdateGlobalWinLimitCriteria : AddGlobalWinLimitCriteria
    {
        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CurrencySettingUpd";
        }
    }
}
