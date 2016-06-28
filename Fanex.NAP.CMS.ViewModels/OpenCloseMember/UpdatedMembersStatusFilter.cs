// --------------------------------------------------------------------------
// <copyright file="UpdatedMembersStatusFilter.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.14">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.OpenCloseMember
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// View model for updated status of members.
    /// </summary>
    [Serializable]
    public class UpdatedMembersStatusFilter
    {
        public List<UpdatedMemberStatus> MemberStatusList { get; set; }

        public byte UpdateType { get; set; }

        public string SearchedUsername { get; set; }

        public int Status { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public string IP { get; set; }

        public int AdminId { get; set; }

        public string AdminUsername { get; set; }
    }
}