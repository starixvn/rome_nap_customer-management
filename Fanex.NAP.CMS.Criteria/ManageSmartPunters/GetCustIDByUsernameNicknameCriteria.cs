// --------------------------------------------------------------------------
// <copyright file="GetCustIDByUsernameNicknameCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.23">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ManageSmartPunters
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Getting customer ID by username or nickname.
    /// </summary>
    public class GetCustIDByUsernameNicknameCriteria : CriteriaBase
    {
        public string Username { get; set; }

        public int CustID { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "Nap_CustomerSelIDByUserNameNickName";
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