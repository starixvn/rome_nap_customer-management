// --------------------------------------------------------------------------
// <copyright file="ConfigureTOByEventService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.16">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Implements
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.CMS.Criteria.ConfigureTOByEvent;
    using Fanex.NAP.CMS.Models.ConfigureTOByEvent;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.ConfigureTOByEvent;

    /// <summary>
    /// Configure turnover by event service.
    /// </summary>
    public class ConfigureTOByEventService : CMSBaseService, IConfigureTOByEventService
    {
        /// <summary>
        /// Gets the sport turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SportTurnover objects.</returns>
        public IEnumerable<SportTurnover> GetSportTurnover(GetEventTurnoverCriteria criteria)
        {
            IEnumerable<SportTurnoverInfo> rawData = this.GetData<SportTurnoverInfo>(criteria);
            IEnumerable<SportTurnover> result = null;

            if (rawData != null && rawData.Any())
            {
                result = rawData.Select(obj => new SportTurnover(obj));
            }

            return result;
        }

        /// <summary>
        /// Gets the league turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of LeagueTurnover objects.</returns>
        public IEnumerable<LeagueTurnover> GetLeagueTurnover(GetEventTurnoverCriteria criteria)
        {
            IEnumerable<LeagueTurnoverInfo> rawData = this.GetData<LeagueTurnoverInfo>(criteria);
            IEnumerable<LeagueTurnover> result = null;

            if (rawData != null && rawData.Any())
            {
                result = rawData.Select(obj => new LeagueTurnover(obj)).OrderBy(obj => obj.LeagueName).ToList();
            }

            return result;
        }
    }
}