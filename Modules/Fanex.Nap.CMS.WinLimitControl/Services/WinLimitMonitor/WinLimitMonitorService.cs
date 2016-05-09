namespace Fanex.Nap.CMS.WinLimitControl.Services.WinLimitMonitor
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Infrastructure.Models;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor;
    using Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor;
    using Fanex.Nap.CMS.WinLimitControl.Repositories.WinLimitMonitor;

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
            var fromDate = criteria.FromDate;
            var toDate = criteria.ToDate;

            criteria.FromDate = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 0, 0, 0);
            criteria.ToDate = new DateTime(toDate.Year, toDate.Month, toDate.Day, 23, 59, 59);

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
