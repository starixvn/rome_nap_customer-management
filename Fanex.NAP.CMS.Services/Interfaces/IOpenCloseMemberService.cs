// --------------------------------------------------------------------------
// <copyright file="IOpenCloseMemberService.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.14">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Interfaces
{
    using Fanex.NAP.CMS.Criteria.OpenCloseMember;
    using Fanex.NAP.CMS.ViewModels.OpenCloseMember;

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