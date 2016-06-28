// --------------------------------------------------------------------------
// <copyright file="UpdateMembersStatusCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.OpenCloseMember
{
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria that contain the list of members that needed to update.
    /// </summary>
    public class UpdateMembersStatusCriteria : CriteriaBase
    {
        public string CustIDs { get; set; }

        public int UserID { get; set; }

        public byte UpdateType { get; set; }

        public int UpdatedCount { get; set; }

        public string Language { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public string IP { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_Misc_OpenCloseMembers_UPD";
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