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
    using System.Linq;
    using Fanex.NAP.CMS.Criteria.Common;
    using Fanex.NAP.CMS.Criteria.TurnoverMonitor;
    using Fanex.NAP.CMS.Models.Common;
    using Fanex.NAP.CMS.Models.TurnoverMonitor;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.TurnoverMonitor;

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
        /// Gets the SMA turnover monitor list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SMATurnoverGroup objects.</returns>
        public IEnumerable<SMATurnoverGroup> GetSMATurnoverMonitorList(GetSMATurnoverMonitorListCriteria criteria)
        {
            IEnumerable<SMATurnoverInfo> rawData = null;
            List<SMATurnoverGroup> result = null;

            rawData = this.GetData<SMATurnoverInfo>(criteria);

            if (rawData != null && rawData.Any())
            {
                IEnumerable<IGrouping<int, SMATurnoverInfo>> groups = rawData.GroupBy(obj => obj.MatchID);
                SMATurnoverGroup turnoverGroup = null;
                SMATurnoverInfo firstItem = null;

                result = new List<SMATurnoverGroup>();

                foreach (IGrouping<int, SMATurnoverInfo> group in groups)
                {
                    firstItem = group.ElementAt(0);
                    turnoverGroup = new SMATurnoverGroup(firstItem);

                    foreach (SMATurnoverInfo item in group)
                    {
                        turnoverGroup.SMAItems.Add(new SMATurnoverItem(item));
                    }

                    result.Add(turnoverGroup);
                }

                result = result.OrderBy(obj => obj.MatchName).ToList();
            }

            return result;
        }
    }
}