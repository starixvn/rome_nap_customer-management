// --------------------------------------------------------------------------
// <copyright file="IOpenCloseMemberService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.14">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.Services.Interfaces
{
    using Fanex.NAP.Miscellany.Criteria.OpenCloseMember;
    using Fanex.NAP.Miscellany.ViewModels.OpenCloseMember;

    public interface IOpenCloseMemberService
    {
        /// <summary>
        /// Gets the members' status of a super/master/agent.
        /// </summary>
        /// <param name="filter">The filter conditions.</param>
        /// <returns>Queried result.</returns>
        GettingMembersStatusResult GetMembersStatus(GettingMembersStatusFilter filter);

        /// <summary>
        /// Updates members' status.
        /// </summary>
        /// <param name="filter">The filter contains the updated data.</param>
        /// <returns>Updated result.</returns>
        UpdatedMembersStatusResult UpdateMembersStatus(UpdatedMembersStatusFilter filter);
    }
}