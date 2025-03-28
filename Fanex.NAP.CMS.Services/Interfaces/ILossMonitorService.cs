namespace Fanex.NAP.CMS.Services.Interfaces
{
    using System.Collections.Generic;
    using Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Framework.Command;
    using Models.LossMonitor;
    using ViewModels.LossMonitor;

    public interface ILossMonitorService
    {
        IEnumerable<SMALossGroup> GetCustomerLoss();

        ICommandResult InsertSMA(CommandBaseCriteria criteria);

        ICommandResult DeleteSMA(CommandBaseCriteria criteria);

        ICommandResult UpdateMaxLoss(CommandBaseCriteria criteria);

        ICommandResult UpdateLetsTalkNotification(CommandBaseCriteria criteria);

        IEnumerable<SMALossInfo> GetGroupDownlines(ICriteria criteria);
    }
}