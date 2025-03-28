// --------------------------------------------------------------------------
// <copyright file="DeleteRequestCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ConfigureCustomerAccess
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    /// <summary>
    /// The criteria that are used to delete a request.
    /// </summary>
    public class DeleteRequestCriteria : CommandBaseCriteria
    {
        public int RequestID { get; set; }

        public int DeletedBy { get; set; }

        public string IPAction { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "Member_LoginVerifyDel";
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