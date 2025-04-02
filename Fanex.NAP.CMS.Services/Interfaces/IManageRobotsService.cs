// --------------------------------------------------------------------------
// <copyright file="IManageRobotsService.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.30">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Interfaces
{
    using Fanex.NAP.CMS.Criteria.ManageRobots;
    using Fanex.NAP.CMS.ViewModels.ManageRobots;
    using Fanex.NAP.CMS.Models.Common;

    public interface IManageRobotsService
    {
        /// <summary>
        /// Gets the robot accounts.
        /// </summary>
        /// <param name="criteria">The criteria that are used to get data.</param>
        /// <returns>GettingRobotAccountResult objects.</returns>
        GettingRobotAccountResult GetRobotAccounts(GetRobotAccountsCriteria criteria);

        /// <summary>
        /// Updates the robot information.
        /// </summary>
        /// <param name="filter">The filter that contains the updated data.</param>
        /// <returns>GettingRobotAccountResult object.</returns>
        GettingRobotAccountResult UpdateRobotInfo(UpdateRobotInfoFilter filter);

        /// <summary>
        /// Adds new robot information.
        /// </summary>
        /// <param name="filter">The filter that contains the new robot data.</param>
        /// <returns>GettingRobotAccountResult object.</returns>
        GettingRobotAccountResult AddRobotAccount(AddRobotInfoFilter filter);

        /// <summary>
        /// Updates robot status.
        /// </summary>
        /// <param name="filter">The filter that contains the robot status.</param>
        /// <returns>GettingRobotAccountResult object.</returns>
        GettingRobotAccountResult UpdateRobotStatus(UpdateRobotStatusFilter filter);
    }
}