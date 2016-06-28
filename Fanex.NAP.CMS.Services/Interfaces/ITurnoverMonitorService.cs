// --------------------------------------------------------------------------
// <copyright file="ITurnoverMonitorService.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.16">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.NAP.CMS.Criteria.Common;
    using Fanex.NAP.CMS.Criteria.TurnoverMonitor;
    using Fanex.NAP.CMS.Models.Common;
    using Fanex.NAP.CMS.ViewModels.TurnoverMonitor;

    public interface ITurnoverMonitorService
    {
        /// <summary>
        /// Gets the sport list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SportInfo object.</returns>
        IEnumerable<SportInfo> GetSportList(GetSportsCriteria criteria);

        /// <summary>
        /// Gets the leagues of sport.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of LeagueInfo objects.</returns>
        IEnumerable<LeagueInfo> GetLeaguesOfSport(GetLeaguesOfSportCriteria criteria);

        /// <summary>
        /// Gets the SMA turnover monitor list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>SMATurnoverReport object contains live and non-live data.</returns>
        SMATurnoverReport GetSMATurnoverMonitorList(GetSMATurnoverMonitorListCriteria criteria);
    }
}