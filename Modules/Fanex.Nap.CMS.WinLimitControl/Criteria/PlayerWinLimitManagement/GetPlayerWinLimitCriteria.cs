namespace Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerWinLimitManagement
{
    using Fanex.Nap.CMS.WinLimitControl.Criteria.GlobalSetting;

    public class GetPlayerWinLimitCriteria : GetGlobalWinLimitByCurrencyGameCriteria
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
