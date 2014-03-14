// --------------------------------------------------------------------------
// <copyright file="GettingMembersStatusFilter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.14">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.ViewModels.OpenCloseMember
{
    using System;

    /// <summary>
    /// Getting members' status filter conditions.
    /// </summary>
    [Serializable]
    public class GettingMembersStatusFilter
    {
        public string Username { get; set; }

        public int Status { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}