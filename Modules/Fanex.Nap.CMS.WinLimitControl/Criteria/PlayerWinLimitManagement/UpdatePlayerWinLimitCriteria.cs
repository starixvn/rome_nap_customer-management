namespace Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerWinLimitManagement
{
    public class UpdatePlayerWinLimitCriteria : AddPlayerWinLimitCriteria
    {
        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CustSettingUpd";
        }
    }
}
