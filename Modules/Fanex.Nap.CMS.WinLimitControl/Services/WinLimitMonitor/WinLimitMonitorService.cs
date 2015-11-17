namespace Fanex.Nap.CMS.WinLimitControl.Services.WinLimitMonitor
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor;
    using Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor;
    using Fanex.Nap.CMS.WinLimitControl.Repositories.WinLimitMonitor;
    using Fanex.Nap.CMS.WinLimitControl.Resources;

    public class WinLimitMonitorService : IWinLimitMonitorService
    {
        private readonly IWinLimitMonitorRepository _winLimitMonitorRepository;

        public WinLimitMonitorService(
            IWinLimitMonitorRepository winLimitMonitorRepository = null)
        {
            _winLimitMonitorRepository = winLimitMonitorRepository ?? new WinLimitMonitorRepository();
        }

        public IList<BlockedWinLimitModel> GetBlockedWinLimits(GetWinLimitMonitorCriteria criteria)
        {
            return _winLimitMonitorRepository.GetBlockedWinLimits(criteria);
        }

        public IList<UnblockedWinLimitModel> GetUnblockedWinLimits(GetWinLimitMonitorCriteria criteria)
        {
            return _winLimitMonitorRepository.GetUnblockedWinLimits(criteria);
        }

        public Result BlockUnblockPlayers(BlockUnblockPlayersCriteria criteria)
        {
            var result = _winLimitMonitorRepository.BlockUnblockPlayers(criteria);
            if (result.Code == 0)
            {
                result.Message = criteria.IsBlock ?
                    WinLimitControlLabels.WinLimitMonitor_BlockAccountsSuccess :
                    WinLimitControlLabels.WinLimitMonitor_UnblockAccountsSuccess;
            }

            return result;
        }

        public IList<PlayerWinLimitHistory> GetWinLimitHistories(ICriteria criteria)
        {
            return _winLimitMonitorRepository.GetWinLimitHistories(criteria);
        }
    }
}
