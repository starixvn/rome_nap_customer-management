// --------------------------------------------------------------------------
// <copyright file="UpdateRequestCriteria.cs">
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
    /// The criteria that contain the updated data of a request.
    /// </summary>
    public class UpdateRequestCriteria : CommandBaseCriteria
    {
        public int RequestId { get; set; }

        public int CustId { get; set; }

        public string IPRequest { get; set; }

        public string CountryCode { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string Remark { get; set; }

        public int UpdatedBy { get; set; }

        public string IPAction { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "Member_LoginVerifyUpd";
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