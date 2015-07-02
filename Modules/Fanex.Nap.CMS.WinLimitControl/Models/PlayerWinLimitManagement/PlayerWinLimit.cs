namespace Fanex.Nap.CMS.WinLimitControl.Models.PlayerWinLimitManagement
{
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Models.GlobalSetting;

    public class PlayerWinLimit : GlobalWinLimit
    {
        public BaseSetting Player { get; set; }
    }
}
