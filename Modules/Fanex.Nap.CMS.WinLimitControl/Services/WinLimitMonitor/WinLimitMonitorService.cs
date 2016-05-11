namespace Fanex.Nap.CMS.WinLimitControl.Services.WinLimitMonitor
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor;
    using Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor;
    using Fanex.Nap.CMS.WinLimitControl.Repositories.WinLimitMonitor;
    using Infrastructure.Models;

    public class WinLimitMonitorService : IWinLimitMonitorService
    {
        private readonly IWinLimitMonitorRepository _winLimitMonitorRepository;

        public WinLimitMonitorService(
            IWinLimitMonitorRepository winLimitMonitorRepository = null)
        {
            _winLimitMonitorRepository = winLimitMonitorRepository ?? new WinLimitMonitorRepository();
        }

        public IList<WinLimitModel> GetWinLimits(GetWinLimitMonitorCriteria criteria)
        {
            return _winLimitMonitorRepository.GetWinLimits(criteria);
        }

        public IList<PlayerWinLimitHistory> GetWinLimitHistories(ICriteria criteria)
        {
            return _winLimitMonitorRepository.GetWinLimitHistories(criteria);
        }

        public Result UnblockPlayer(ICriteria criteria)
        {
            return _winLimitMonitorRepository.UnblockPlayer(criteria);
        }
    }
}
