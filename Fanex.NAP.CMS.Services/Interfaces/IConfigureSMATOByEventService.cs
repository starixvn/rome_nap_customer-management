// --------------------------------------------------------------------------
// <copyright file="IConfigureSMATOByEventService.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.16">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.NAP.CMS.Criteria.Common;
    using Fanex.NAP.CMS.Criteria.ConfigureSMATOByEvent;
    using Fanex.NAP.CMS.Models.Common;
    using Fanex.NAP.CMS.ViewModels.ConfigureSMATOByEvent;
    using Fanex.NAP.CMS.Models.Common;

    public interface IConfigureSMATOByEventService
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
        /// Gets the SMA turnover by event.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SMATurnoverByEventGroup objects.</returns>
        IEnumerable<SMATurnoverByEventGroup> GetSMATurnoverByEvent(GetSMATurnoverByEventCriteria criteria);

        /// <summary>
        /// Updates the turnover.
        /// </summary>
        /// <param name="criteria">The criteria contain updated data.</param>
        /// <returns>ICommandResult indicates the updating process is successful or not.</returns>
        ICommandResult UpdateTurnover(CommandBaseCriteria criteria);
    }
}