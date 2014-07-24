// --------------------------------------------------------------------------
// <copyright file="ConfigureSMATOByEventService.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.Data.Repository;
    using Fanex.NAP.CMS.Criteria.Common;
    using Fanex.NAP.CMS.Criteria.ConfigureSMATOByEvent;
    using Fanex.NAP.CMS.Models.Common;
    using Fanex.NAP.CMS.Models.ConfigureSMATOByEvent;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.ConfigureSMATOByEvent;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Configure SMA turnover by match service.
    /// </summary>
    public class ConfigureSMATOByEventService : CMSBaseService, IConfigureSMATOByEventService
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
        /// Gets the SMA turnover by event.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SMATurnoverByEventGroup objects.</returns>
        public IEnumerable<SMATurnoverByEventGroup> GetSMATurnoverByEvent(GetSMATurnoverByEventCriteria criteria)
        {
            IEnumerable<SMATurnoverByEventInfo> rawData = null;
            List<SMATurnoverByEventGroup> result = null;

            rawData = this.Repository.Fetch<SMATurnoverByEventInfo>(criteria);

            if (rawData != null && rawData.Any())
            {
                IEnumerable<IGrouping<int, SMATurnoverByEventInfo>> groups = rawData.GroupBy(obj => obj.MatchID);
                SMATurnoverByEventGroup turnoverGroup = null;
                SMATurnoverByEventInfo firstItem = null;

                result = new List<SMATurnoverByEventGroup>();

                foreach (IGrouping<int, SMATurnoverByEventInfo> group in groups)
                {
                    firstItem = group.ElementAt(0);
                    turnoverGroup = new SMATurnoverByEventGroup(firstItem);

                    if (firstItem.CustID.HasValue)
                    {
                        foreach (SMATurnoverByEventInfo item in group)
                        {
                            turnoverGroup.SMAItems.Add(new SMATurnoverByEvent(item));
                        }
                    }

                    result.Add(turnoverGroup);
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the turnover.
        /// </summary>
        /// <param name="criteria">The criteria contain updated data.</param>
        /// <returns>ICommandResult indicates the updating process is successful or not.</returns>
        public ICommandResult UpdateTurnover(ICriteria criteria)
        {
            return this.UpdateData(criteria);
        }
    }
}