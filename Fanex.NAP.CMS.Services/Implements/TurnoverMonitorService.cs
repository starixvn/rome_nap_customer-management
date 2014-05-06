// --------------------------------------------------------------------------
// <copyright file="TurnoverMonitorService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.16">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Implements
{
    using System.Collections.Generic;
    using Fanex.NAP.CMS.Core.Resources;
    using Fanex.NAP.CMS.Core.Web;
    using Fanex.NAP.CMS.Criteria.Common;
    using Fanex.NAP.CMS.Models.Common;
    using Fanex.NAP.CMS.Services.Interfaces;

    /// <summary>
    /// Turnover monitor service.
    /// </summary>
    public class TurnoverMonitorService : CMSBaseService, ITurnoverMonitorService
    {
        /// <summary>
        /// Gets the sport list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SportInfo object.</returns>
        public IEnumerable<SportInfo> GetSportList(GetSportsCriteria criteria)
        {
            IEnumerable<SportInfo> result = this.GetData<SportInfo>(criteria);

            return result;
        }

        /// <summary>
        /// Gets the leagues of sport.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of LeagueInfo objects.</returns>
        public IEnumerable<LeagueInfo> GetLeaguesOfSport(GetLeaguesOfSportCriteria criteria)
        {
            IEnumerable<LeagueInfo> result = this.GetData<LeagueInfo>(criteria);

            return result;
        }

        /// <summary>
        /// Gets the matches of league.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of MatchInfo objects.</returns>
        public IEnumerable<MatchInfo> GetMatchesOfLeague(GetMatchesOfLeagueCriteria criteria)
        {
            IEnumerable<MatchInfo> result = this.GetData<MatchInfo>(criteria);

            foreach (MatchInfo match in result)
            {
                match.MatchName = string.Format("{0} {1} {2}", CacheRef.GetTeamName(match.HomeID), ViewLang.Vs, CacheRef.GetTeamName(match.AwayID));
            }

            return result;
        }
    }
}