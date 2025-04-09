// --------------------------------------------------------------------------
// <copyright file="DeleteSmartPunterCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.11">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ManageSmartPunters
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    /// <summary>
    /// Deleting smart punter criteria.
    /// </summary>
    public class DeleteSmartPunterCriteria : CommandBaseCriteria
    {
        public int CustID { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "Nap_CustomerDelSmartPunter";
        }

        /// <summary>
        /// Determines whether this criteria is valid.
        /// </summary>
        /// <returns>Whether this criteria is valid or not.</returns>
        public override bool IsValid()
        {
            return this.CustID > 0 && !string.IsNullOrEmpty(this.IP) && this.UserID > 0;
        }
    }
}