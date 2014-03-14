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
    using System;
    using Fanex.NAP.Miscellany.Models.OpenCloseMember;

    /// <summary>
    /// View model contains the status of a member.
    /// </summary>
    [Serializable]
    public class MemberStatus
    {
        public MemberStatus()
        {
        }

        public MemberStatus(MemberStatusInfo rawData)
        {
            if (rawData != null)
            {
                this.CustId = rawData.CustId;
                this.Username = rawData.Member;
                this.Closed = rawData.Closed;
            }
        }

        public int CustId { get; set; }

        public string Username { get; set; }

        public bool Closed { get; set; }

        public bool IsChecked { get; set; }
    }
}