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
        /// Get the members' status of a super/master/agent.
        /// </summary>
        /// <param name="criteria">The filter conditions.</param>
        /// <returns>Queried result.</returns>
        GettingMembersStatusResult GetMembersStatus(GetMembersStatusCriteria criteria);
    }
}