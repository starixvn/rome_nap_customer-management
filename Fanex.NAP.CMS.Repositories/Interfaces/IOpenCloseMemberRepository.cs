// --------------------------------------------------------------------------
// <copyright file="IOpenCloseMemberRepository.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.3">Created</change>
// </history>

namespace Fanex.NAP.CMS.Repositories.Interfaces
{
    using Fanex.NAP.CMS.Criteria.OpenCloseMember;
    using Fanex.NAP.CMS.Models.OpenCloseMember;
    using System.Collections.Generic;

    public interface IOpenCloseMemberRepository
    {
        /// <summary>
        /// Gets the members' status data.
        /// </summary>
        /// <param name="criteria">The criteria that are use to query data.</param>
        /// <returns>List of MemberStatusInfo objects.</returns>
        IEnumerable<MemberStatusInfo> GetMembersStatusData(GetMembersStatusCriteria criteria);

        UpdateResult UpdateMembersStatus(UpdateMembersStatusCriteria criteria);
    }
}