// --------------------------------------------------------------------------
// <copyright file="UpdatedMembersStatusResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.17">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.ViewModels.OpenCloseMember
{
    using System;

    /// <summary>
    /// View model represents the result of a updating members' status call.
    /// </summary>
    [Serializable]
    public class UpdatedMembersStatusResult
    {
        public GettingMembersStatusResult MembersStatusResult { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public int NumberOfUpdatedItems { get; set; }

        public int NumberOfFailedItems { get; set; }

        public int TotalItems { get; set; }
    }
}