// --------------------------------------------------------------------------
// <copyright file="MemberStatus.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.12">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.ViewModels.OpenCloseMember
{
    /// <summary>
    /// View model contains the status of a member.
    /// </summary>
    public class MemberStatus
    {
        public int CustId { get; set; }

        public string Username { get; set; }

        public bool IsOpen { get; set; }

        public bool IsChecked { get; set; }
    }
}