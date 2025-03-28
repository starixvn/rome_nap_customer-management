// --------------------------------------------------------------------------
// <copyright file="UpdateRobotStatusCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.2">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ManageRobots
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    /// <summary>
    /// The criteria that are used to update status of robots.
    /// </summary>
    public class UpdateRobotStatusCriteria : CommandBaseCriteria
    {
        public string CustIDs { get; set; }

        public bool Status { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_CustomerUpdStatusRobotAccount";
        }

        /// <summary>
        /// Determines whether this criteria is valid.
        /// </summary>
        /// <returns>Whether this criteria is valid or not.</returns>
        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(CustIDs) && !string.IsNullOrEmpty(IP) && UserID > 0;
        }
    }
}