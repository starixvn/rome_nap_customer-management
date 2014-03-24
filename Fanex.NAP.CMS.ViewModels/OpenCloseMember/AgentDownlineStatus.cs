// --------------------------------------------------------------------------
// <copyright file="AgentDownlineStatus.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.12">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.OpenCloseMember
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// View model contains list of member's status object.
    /// </summary>
    [Serializable]
    public class AgentDownlineStatus
    {
        public AgentDownlineStatus()
        {
            this.MemberStatusList = new List<MemberStatus>();
        }

        public int AgentId { get; set; }

        public string Username { get; set; }

        public List<MemberStatus> MemberStatusList { get; set; }

        /// <summary>
        /// Adds the member to the members' status list.
        /// </summary>
        /// <param name="member">The member.</param>
        public void AddMember(MemberStatus member)
        {
            if (member != null)
            {
                this.MemberStatusList.Add(member);
            }
        }
    }
}