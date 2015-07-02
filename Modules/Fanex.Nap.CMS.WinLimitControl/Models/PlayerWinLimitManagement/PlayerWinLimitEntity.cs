namespace Fanex.Nap.CMS.WinLimitControl.Models.PlayerWinLimitManagement
{
    using Fanex.Nap.CMS.Settings.Models;
    using Fanex.Nap.CMS.WinLimitControl.Models.GlobalSetting;

    public class PlayerWinLimitEntity : WinLimitEntity
    {
        public BaseSetting Player { get; set; }
    }
}
