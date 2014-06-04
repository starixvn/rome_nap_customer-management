// --------------------------------------------------------------------------
// <copyright file="GetRobotAccountsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.30">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ManageRobots
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria that are used to get robot account.
    /// </summary>
    public class GetRobotAccountsCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public bool? Status { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        [SpParam(Ignored = true)]
        public int ErrorCode { get; set; }

        [SpParam(Ignored = true)]
        public string ErrorMsg { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_CustomerSelRobotAccounts";
        }

        /// <summary>
        /// Determines whether this criteria is valid.
        /// </summary>
        /// <returns>Whether this criteria is valid or not.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}