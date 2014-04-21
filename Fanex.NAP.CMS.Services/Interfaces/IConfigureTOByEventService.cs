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
        /// <returns>List of LeagueTurnover objects.</returns>
        IEnumerable<LeagueTurnover> GetLeagueTurnover(GetEventTurnoverCriteria criteria);
    }
}