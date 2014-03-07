// --------------------------------------------------------------------------
// <copyright file="RequestInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.Models.AllowAccessManagement
{
    using System;

    /// <summary>
    /// View model for new/updated request.
    /// </summary>
    public class RequestInfo
    {
        public int RequestId { get; set; }

        public DateTime ExpiredDate { get; set; }

        public string Country { get; set; }

        public string IP { get; set; }

        public string Remark { get; set; }

        public string Creator { get; set; }

        public DateTime CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}