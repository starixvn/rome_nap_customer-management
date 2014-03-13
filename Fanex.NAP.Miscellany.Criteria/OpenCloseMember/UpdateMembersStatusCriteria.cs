// --------------------------------------------------------------------------
// <copyright file="UpdateMembersStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.Criteria.OpenCloseMember
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria that contain the list of members that needed to update.
    /// </summary>
    public class UpdateMembersStatusCriteria : CriteriaBase
    {
        public string CustIdList { get; set; }

        public int AdminId { get; set; }

        public string AdminUsername { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "Key";
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