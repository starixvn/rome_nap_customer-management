// --------------------------------------------------------------------------
// <copyright file="InsertSmartPunterCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.11">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ManageSmartPunters
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Inserting new smart punter criteria.
    /// </summary>
    public class InsertSmartPunterCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public string Remark { get; set; }

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
            return "NAP_CustomerInsSmartPunter";
        }

        /// <summary>
        /// Determines whether this criteria is valid.
        /// </summary>
        /// <returns>Whether this criteria is valid or not.</returns>
        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(this.Username) && !string.IsNullOrEmpty(this.IP) && this.UserID > 0;
        }
    }
}