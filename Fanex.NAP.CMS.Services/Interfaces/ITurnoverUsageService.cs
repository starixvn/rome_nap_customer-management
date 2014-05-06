// --------------------------------------------------------------------------
// <copyright file="ITurnoverUsageService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.6">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.NAP.CMS.Criteria.Common;
    using Fanex.NAP.CMS.Criteria.TurnoverUsage;
    using Fanex.NAP.CMS.Models.Common;
    using Fanex.NAP.CMS.ViewModels.TurnoverUsage;

    public interface ITurnoverUsageService
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
        /// Gets the matches of league.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of MatchInfo objects.</returns>
        IEnumerable<MatchInfo> GetMatchesOfLeague(GetMatchesOfLeagueCriteria criteria);

        /// <summary>
        /// Gets the turnover usage report.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of TurnoverUsageItem objects.</returns>
        IEnumerable<TurnoverUsageItem> GetTurnoverUsageReport(GetTurnoverUsageCriteria criteria);
    }
}