namespace Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerSetting
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.CurrencySetting;

    public class GetPlayerWinLimitCriteria : GetCurrencyWinLimitCriteria
    {
        public string Username { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalRow { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CustSettingSel";
        }
    }
}
