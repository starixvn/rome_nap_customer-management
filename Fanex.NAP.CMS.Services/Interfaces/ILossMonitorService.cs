namespace Fanex.NAP.CMS.Services.Interfaces
{
    using System.Collections.Generic;
    using Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Models.LossMonitor;
    using ViewModels.LossMonitor;

    public interface ILossMonitorService
    {
        IEnumerable<SMALossGroup> GetCustomerLoss();

        Result InsertSMA(CommandBaseCriteria criteria);

        Result DeleteSMA(CommandBaseCriteria criteria);

        Result UpdateMaxLoss(CommandBaseCriteria criteria);

        Result UpdateLetsTalkNotification(CommandBaseCriteria criteria);

        IEnumerable<SMALossInfo> GetGroupDownlines(ICriteria criteria);
    }
}