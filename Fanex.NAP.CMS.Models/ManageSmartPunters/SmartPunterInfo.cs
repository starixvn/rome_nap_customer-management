// --------------------------------------------------------------------------
// <copyright file="SmartPunterInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.5">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.ManageSmartPunters
{
    using System;

    /// <summary>
    /// Smart punter model.
    /// </summary>
    public class SmartPunterInfo
    {
        public int CustID { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public byte StatusID { get; set; }

        public string Currency { get; set; }

        public string Site { get; set; }

        public string Super { get; set; }

        public string Master { get; set; }

        public string Agent { get; set; }

        public string Creator { get; set; }

        public DateTime? CreateDate { get; set; }

        public string Remark { get; set; }
    }
}