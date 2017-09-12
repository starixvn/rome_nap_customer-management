// --------------------------------------------------------------------------
// <copyright file="ILossMonitorService.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.23">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Interfaces
{
    using Fanex.NAP.CMS.Criteria.LossMonitor;
    using Fanex.NAP.CMS.ViewModels.LossMonitor;
    using Fanex.NAP.Framework.Command;
    using System.Collections.Generic;

    public interface ILossMonitorService
    {
        List<SMALossGroup> GetSMALoss(GetSMALossMonitorListCriteria criteria);

        ICommandResult InsertSMA(InsertSMACriteria criteria);

        ICommandResult DeleteSMA(DeleteSMACriteria criteria);

        ICommandResult UpdateMaxLoss(UpdateSMAMaxLossCriteria criteria);

        List<SMALossItem> GetGroupDownlines(GetGroupDownlinesCriteria criteria);
    }
}