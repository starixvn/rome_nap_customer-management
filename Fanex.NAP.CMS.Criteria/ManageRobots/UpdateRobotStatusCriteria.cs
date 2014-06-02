// --------------------------------------------------------------------------
// <copyright file="UpdateRobotStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.2">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ManageRobots
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria that are used to update status of robots.
    /// </summary>
    public class UpdateRobotStatusCriteria : CriteriaBase
    {
        public string CustIDs { get; set; }

        public bool Status { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "Acc_CustomerUpdStatusRobotAccount";
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