namespace Fanex.NAP.CMS.Services.Interfaces
{
    using System.Collections.Generic;
    using Data.Repository;
    using Framework.Command;
    using Models.LossMonitor;
    using ViewModels.LossMonitor;

    public interface ILossMonitorService
    {
        IEnumerable<SMALossGroup> GetCustomerLoss();

        ICommandResult InsertSMA(ICriteria criteria);

        ICommandResult DeleteSMA(ICriteria criteria);

        ICommandResult UpdateMaxLoss(ICriteria criteria);

        ICommandResult UpdateLetsTalkNotification(ICriteria criteria);

        IEnumerable<SMALossInfo> GetGroupDownlines(ICriteria criteria);
    }
}