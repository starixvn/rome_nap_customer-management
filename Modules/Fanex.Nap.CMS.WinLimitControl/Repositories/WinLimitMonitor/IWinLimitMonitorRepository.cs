namespace Fanex.Nap.CMS.WinLimitControl.Repositories.WinLimitMonitor
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor;
    using Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor;

    public interface IWinLimitMonitorRepository
    {
        WinLimitMonitorDataResult GetWinLimitMonitor(ICriteria criteria);

        Result UnblockAccounts(UnblockAccountsCriteria criteria);
    }
}
