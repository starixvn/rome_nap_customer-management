// --------------------------------------------------------------------------
// <copyright file="RobotAccountInfo.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.30">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.ManageRobots
{
    using System;

    /// <summary>
    /// Robot account model.
    /// </summary>
    public class RobotAccountInfo
    {
        public int CustID { get; set; }

        public string Username { get; set; }

        public bool IsActive { get; set; }

        public string Currency { get; set; }

        public string Site { get; set; }

        public string Super { get; set; }

        public string Master { get; set; }

        public string Agent { get; set; }

        public string Creator { get; set; }

        public DateTime CreateDate { get; set; }

        public string ModifyUser { get; set; }

        public DateTime? LastModifyTime { get; set; }

        public string Remark { get; set; }
    }
}