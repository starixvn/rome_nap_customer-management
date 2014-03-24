// --------------------------------------------------------------------------
// <copyright file="GetCountryListCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.7">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.Common
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria that are used to get country list.
    /// </summary>
    public class GetCountryListCriteria : CriteriaBase
    {
        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "Member_GetAllCountry";
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