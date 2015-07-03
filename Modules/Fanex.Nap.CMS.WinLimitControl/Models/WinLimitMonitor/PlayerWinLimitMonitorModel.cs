namespace Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Settings.Models;

    public class PlayerWinLimitMonitorModel
    {
        public BaseSetting PlayerInfo { get; set; }

        public BaseSetting Currency { get; set; }

        public IList<GameWinLimitMonitorModel> GameWinLimitMonitors { get; set; }
    }
}
