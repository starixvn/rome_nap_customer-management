// --------------------------------------------------------------------------
// <copyright file="TurnoverUsageService.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.6">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Implements
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.CMS.Core.Resources;
    using Fanex.NAP.CMS.Core.Web;
    using Fanex.NAP.CMS.Criteria.Common;
    using Fanex.NAP.CMS.Criteria.TurnoverUsage;
    using Fanex.NAP.CMS.Models.Common;
    using Fanex.NAP.CMS.Models.TurnoverUsage;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.TurnoverUsage;

    /// <summary>
    /// Turnover usage service.
    /// </summary>
    public class TurnoverUsageService : CMSBaseService, ITurnoverUsageService
    {
        /// <summary>
        /// Gets the sport list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SportInfo object.</returns>
        public IEnumerable<SportInfo> GetSportList(GetSportsCriteria criteria)
        {
            IEnumerable<SportInfo> result = this.Repository.Fetch<SportInfo>(criteria);

            return result;
        }

        /// <summary>
        /// Gets the leagues of sport.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of LeagueInfo objects.</returns>
        public IEnumerable<LeagueInfo> GetLeaguesOfSport(GetLeaguesOfSportCriteria criteria)
        {
            IEnumerable<LeagueInfo> result = this.Repository.Fetch<LeagueInfo>(criteria);

            return result;
        }

        /// <summary>
        /// Gets the matches of league.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of MatchInfo objects.</returns>
        public IEnumerable<MatchInfo> GetMatchesOfLeague(GetMatchesOfLeagueCriteria criteria)
        {
            IEnumerable<MatchInfo> result = this.Repository.Fetch<MatchInfo>(criteria);

            foreach (MatchInfo match in result)
            {
                match.MatchName = string.Format("{0} {1} {2}", CacheRef.GetTeamName(match.HomeID), ViewLang.Vs, CacheRef.GetTeamName(match.AwayID));
            }

            return result;
        }

        /// <summary>
        /// Gets the turnover usage report.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of TurnoverUsageItem objects.</returns>
        public IEnumerable<TurnoverUsageItem> GetTurnoverUsageReport(GetTurnoverUsageCriteria criteria)
        {
            IEnumerable<TurnoverUsageInfo> rawData = this.Repository.Fetch<TurnoverUsageInfo>(criteria);
            IEnumerable<TurnoverUsageItem> result = null;

            if (rawData.Any())
            {
                result = rawData.Select(obj => new TurnoverUsageItem(obj));
            }

            return result;
        }
    }
}