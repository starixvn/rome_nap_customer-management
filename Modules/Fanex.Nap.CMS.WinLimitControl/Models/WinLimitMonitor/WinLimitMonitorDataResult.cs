namespace Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor
{
    using System.Collections.Generic;

    public class WinLimitMonitorDataResult
    {
        public IList<WinLimitMonitorEntity> WinLimitMonitorEntities { get; set; }

        public int TotalPlayers { get; set; }
    }
}
