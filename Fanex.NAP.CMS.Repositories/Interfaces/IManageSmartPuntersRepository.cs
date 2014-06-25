// --------------------------------------------------------------------------
// <copyright file="IManageSmartPuntersRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.24">Created</change>
// </history>

namespace Fanex.NAP.CMS.Repositories.Interfaces
{
    using Fanex.NAP.CMS.Criteria.ManageSmartPunters;

    public interface IManageSmartPuntersRepository
    {
        /// <summary>
        /// Gets customer ID by username or nickname.
        /// </summary>
        /// <param name="criteria">The criteria that are use to query data.</param>
        /// <returns>Customer ID.</returns>
        int GetCustIDByUsernameNickname(GetCustIDByUsernameNicknameCriteria criteria);
    }
}