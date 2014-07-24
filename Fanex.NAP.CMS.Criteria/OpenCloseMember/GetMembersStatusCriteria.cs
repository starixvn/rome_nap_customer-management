// --------------------------------------------------------------------------
// <copyright file="GetMembersStatusCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.OpenCloseMember
{
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria that are used to get the member list.
    /// </summary>
    public class GetMembersStatusCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public int IsClosed { get; set; }

        public int ItemPerPage { get; set; }

        public int PageIndex { get; set; }

        public int TotalItems { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public string Language { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_Misc_OpenCloseMembers_List";
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