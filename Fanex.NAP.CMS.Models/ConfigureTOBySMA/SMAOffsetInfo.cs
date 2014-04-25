// --------------------------------------------------------------------------
// <copyright file="SMAOffsetInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.24">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.ConfigureTOBySMA
{
    /// <summary>
    /// Model using to map the SMA offset information.
    /// </summary>
    public class SMAOffsetInfo
    {
        public string Username { get; set; }

        public int CustID { get; set; }

        public byte? RoleID { get; set; }

        public string Currency { get; set; }

        public decimal? Credit { get; set; }

        public int Offset { get; set; }
    }
}