// --------------------------------------------------------------------------
// <copyright file="ISMALossMonitorService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.23">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.NAP.CMS.Criteria.SMALossMonitor;
    using Fanex.NAP.CMS.ViewModels.SMALossMonitor;

    public interface ISMALossMonitorService
    {
        /// <summary>
        /// Gets the SMA loss report.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SMALossGroup objects.</returns>
        List<SMALossGroup> GetSMALoss(GetSMALossMonitorListCriteria criteria);
    }
}