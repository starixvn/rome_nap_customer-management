// --------------------------------------------------------------------------
// <copyright file="MasterDownlineStatus.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.12">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.ViewModels.OpenCloseMember
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// View model contains the member status of down-line agents.
    /// </summary>
    [Serializable]
    public class MasterDownlineStatus
    {
        public MasterDownlineStatus()
        {
            this.AgentStatusList = new List<AgentDownlineStatus>();
        }

        public int MasterId { get; set; }

        public string Username { get; set; }

        public int SiteId { get; set; }

        public List<AgentDownlineStatus> AgentStatusList { get; set; }
    }
}