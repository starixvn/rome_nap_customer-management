// --------------------------------------------------------------------------
// <copyright file="SMATurnoverByEventInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.26">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.ConfigureSMATOByEvent
{
    using System;

    /// <summary>
    /// SMA Turnover model.
    /// </summary>
    public class SMATurnoverByEventInfo
    {
        public int MatchID { get; set; }

        public string MatchCode { get; set; }

        public DateTime? EventDate { get; set; }

        public int HomeID { get; set; }

        public int AwayID { get; set; }

        public int CustID { get; set; }

        public string Username { get; set; }

        public byte RoleID { get; set; }

        public int Currency { get; set; }

        public decimal? Credit { get; set; }

        public int Value { get; set; }

        public decimal? MaxTurnover { get; set; }
    }
}