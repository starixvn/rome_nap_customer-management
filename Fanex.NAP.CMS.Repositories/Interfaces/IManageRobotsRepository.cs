// --------------------------------------------------------------------------
// <copyright file="IManageRobotsRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.2">Created</change>
// </history>

namespace Fanex.NAP.CMS.Repositories.Interfaces
{
    using System.Collections.Generic;
    using Fanex.NAP.CMS.Criteria.ManageRobots;
    using Fanex.NAP.CMS.Models.ManageRobots;

    public interface IManageRobotsRepository
    {
        /// <summary>
        /// Gets robot account data.
        /// </summary>
        /// <param name="criteria">The criteria that are use to query data.</param>
        /// <returns>List of RobotAccountInfo objects.</returns>
        IEnumerable<RobotAccountInfo> GetRobotAccountData(GetRobotAccountsCriteria criteria);
    }
}