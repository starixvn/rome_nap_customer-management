// --------------------------------------------------------------------------
// <copyright file="GettingMembersStatusResult.cs" company="Nexcel Solutions Vietnam">
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
    /// View model represents the result of a getting members' status call.
    /// </summary>
    [Serializable]
    public class GettingMembersStatusResult
    {
        public GettingMembersStatusResult()
        {
            this.DownlineList = new List<MasterDownlineStatus>();
        }

        public List<MasterDownlineStatus> DownlineList { get; set; }

        public int TotalItems { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        /// <summary>
        /// Adds the master.
        /// </summary>
        /// <param name="master">The master.</param>
        public void AddMaster(MasterDownlineStatus master)
        {
            if (master != null)
            {
                this.DownlineList.Add(master);
            }
        }
    }
}