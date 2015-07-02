namespace Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerWinLimitManagement
{
    using Fanex.Nap.CMS.WinLimitControl.Criteria.GlobalSetting;

    public class GetPlayerWinLimitCriteria : GetGlobalWinLimitByCurrencyGameCriteria
    {
        public string Username { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CustSettingSel";
        }
    }
}
