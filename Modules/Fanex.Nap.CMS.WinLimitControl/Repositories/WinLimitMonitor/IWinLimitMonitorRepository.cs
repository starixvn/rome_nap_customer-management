namespace Fanex.Nap.CMS.WinLimitControl.Repositories.WinLimitMonitor
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor;
    using Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor;

    public interface IWinLimitMonitorRepository
    {
        IList<WinLimitModel> GetWinLimits(GetWinLimitMonitorCriteria criteria);

        Result BlockUnblockPlayers(ICriteria criteria);

        IList<PlayerWinLimitHistory> GetWinLimitHistories(ICriteria criteria);
    }
}
