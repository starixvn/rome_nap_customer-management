// --------------------------------------------------------------------------
// <copyright file="MemberStatusInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.14">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.OpenCloseMember
{
    /// <summary>
    /// Model contains the member status information.
    /// </summary>
    public class MemberStatusInfo
    {
        public int CustId { get; set; }

        public string Member { get; set; }

        public string Master { get; set; }

        public string Agent { get; set; }

        public bool SyncCasino { get; set; }

        public bool Closed { get; set; }
    }
}