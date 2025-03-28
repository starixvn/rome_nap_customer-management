// --------------------------------------------------------------------------
// <copyright file="InsertRequestCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ConfigureCustomerAccess
{
    using System;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    /// <summary>
    /// The criteria that contain the data of new request.
    /// </summary>
    public class InsertRequestCriteria : CommandBaseCriteria
    {
        public string Username { get; set; }

        public string IPRequest { get; set; }

        public string CountryCode { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string Remark { get; set; }

        public int Creator { get; set; }

        public string IPAction { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "Member_LoginVerifyIns";
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