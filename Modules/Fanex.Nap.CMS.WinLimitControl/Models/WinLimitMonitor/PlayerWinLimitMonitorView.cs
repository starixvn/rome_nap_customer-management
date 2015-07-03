namespace Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor
{
    using System.Collections.Generic;

    public class PlayerWinLimitMonitorView
    {
        public IList<PlayerWinLimitMonitorModel> PlayerWinLimitMonitorModels { get; set; }

        public int TotalPlayers { get; set; }
    }
}
