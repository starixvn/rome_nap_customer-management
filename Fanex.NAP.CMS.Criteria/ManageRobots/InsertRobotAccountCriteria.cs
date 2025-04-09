// --------------------------------------------------------------------------
// <copyright file="InsertRobotAccountCriteria.cs">
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
    /// The criteria that are used to add new robot.
    /// </summary>
    public class InsertRobotAccountCriteria : CommandBaseCriteria
    {
        public string Usernames { get; set; }

        public bool Status { get; set; }

        public string Remark { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_CustomerInsRobotAccount";
        }

        /// <summary>
        /// Determines whether this criteria is valid.
        /// </summary>
        /// <returns>Whether this criteria is valid or not.</returns>
        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(Usernames) && !string.IsNullOrEmpty(IP) && UserID > 0;
        }
    }
}