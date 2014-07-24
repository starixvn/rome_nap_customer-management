// --------------------------------------------------------------------------
// <copyright file="GetRequestInfoListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.7">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ConfigureCustomerAccess
{
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria that are used to get list of requests.
    /// </summary>
    public class GetRequestInfoListCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "Member_LoginVerifySelList";
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