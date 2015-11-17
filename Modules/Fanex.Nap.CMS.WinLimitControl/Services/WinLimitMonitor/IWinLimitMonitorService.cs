namespace Fanex.Nap.CMS.WinLimitControl.Services.WinLimitMonitor
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor;
    using Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor;

    public interface IWinLimitMonitorService
    {
        IList<BlockedWinLimitModel> GetBlockedWinLimits(GetWinLimitMonitorCriteria criteria);

        IList<UnblockedWinLimitModel> GetUnblockedWinLimits(GetWinLimitMonitorCriteria criteria);

        Result BlockUnblockPlayers(BlockUnblockPlayersCriteria criteria);

        IList<PlayerWinLimitHistory> GetWinLimitHistories(ICriteria criteria);
    }
}
