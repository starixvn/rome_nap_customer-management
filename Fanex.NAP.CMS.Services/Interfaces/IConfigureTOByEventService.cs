// --------------------------------------------------------------------------
// <copyright file="IConfigureTOByEventService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.16">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.NAP.CMS.Criteria.ConfigureTOByEvent;
    using Fanex.NAP.CMS.Models.ConfigureTOByEvent;
    using Fanex.NAP.CMS.ViewModels.ConfigureTOByEvent;

    public interface IConfigureTOByEventService
    {
        /// <summary>
        /// Gets the sport turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SportTurnover objects.</returns>
        IEnumerable<SportTurnover> GetSportTurnover(GetEventTurnoverCriteria criteria);

        /// <summary>
        /// Gets the league turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>GotLeagueTurnoverResult object.</returns>
        GotLeagueTurnoverResult GetLeagueTurnover(GetEventTurnoverCriteria criteria);

        /// <summary>
        /// Gets the match turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>GotMatchTurnoverResult object.</returns>
        GotMatchTurnoverResult GetMatchTurnover(GetEventTurnoverCriteria criteria);

        /// <summary>
        /// Updates sport maximum turnover and get the list of sport turnovers after updating.
        /// </summary>
        /// <param name="filter">The filter that is used to update and query data.</param>
        /// <returns>UpdatedTurnoverResult objects.</returns>
        UpdatedTurnoverResult UpdateSportTurnover(UpdateSportTurnoverFilter filter);

        /// <summary>
        /// Updates league maximum turnover and get the list of league turnovers after updating.
        /// </summary>
        /// <param name="filter">The filter that is used to update and query data.</param>
        /// <returns>UpdatedTurnoverResult objects.</returns>
        UpdatedTurnoverResult UpdateLeagueTurnover(UpdateLeagueTurnoverFilter filter);

        /// <summary>
        /// Updates match maximum turnover and get the list of match turnovers after updating.
        /// </summary>
        /// <param name="filter">The filter that is used to update and query data.</param>
        /// <returns>UpdatedTurnoverResult objects.</returns>
        UpdatedTurnoverResult UpdateMatchTurnover(UpdateMatchTurnoverFilter filter);

        /// <summary>
        /// Remove league exception and get the list of league turnovers after updating.
        /// </summary>
        /// <param name="filter">The filter that is used to update and query data.</param>
        /// <returns>UpdatedTurnoverResult objects.</returns>
        UpdatedTurnoverResult RemoveLeagueException(RemoveLeagueExceptionFilter filter);

        /// <summary>
        /// Remove match exception and get the list of match turnovers after updating.
        /// </summary>
        /// <param name="filter">The filter that is used to update and query data.</param>
        /// <returns>UpdatedTurnoverResult objects.</returns>
        UpdatedTurnoverResult RemoveMatchException(RemoveMatchExceptionFilter filter);

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
    }
}