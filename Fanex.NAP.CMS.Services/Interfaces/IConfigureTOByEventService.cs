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
    using Fanex.NAP.CMS.Models.Common;
    using Fanex.NAP.CMS.Models.ConfigureTOByEvent;
    using Fanex.NAP.CMS.ViewModels.ConfigureTOByEvent;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Framework.Data;

    public interface IConfigureTOByEventService
    {
        /// <summary>
        /// Gets the sport turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SportTurnover objects.</returns>
        IEnumerable<SportTurnover> GetSportTurnover(GetSportTurnoverCriteria criteria);

        /// <summary>
        /// Gets the league turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>GotLeagueTurnoverResult object.</returns>
        GotLeagueTurnoverResult GetLeagueTurnover(GetLeagueTurnoverCriteria criteria);

        /// <summary>
        /// Gets the match turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>GotMatchTurnoverResult object.</returns>
        GotMatchTurnoverResult GetMatchTurnover(GetMatchTurnoverCriteria criteria);

        /// <summary>
        /// Updates the turnover.
        /// </summary>
        /// <param name="criteria">The criteria contain updated data.</param>
        /// <returns>ICommandResult indicates the updating process is successful or not.</returns>
        ICommandResult UpdateTurnover(ICriteria criteria);

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
        /// Gets the list of countries which have active leagues.
        /// </summary>
        /// <returns>List of CountryInfo objects.</returns>
        IEnumerable<CountryInfo> GetCountryList();

        /// <summary>
        /// Gets the list of special leagues.
        /// </summary>
        /// <returns>List of CountryInfo objects.</returns>
        IEnumerable<SpecialLeagueInfo> GetSpecialLeagueList();
    }
}