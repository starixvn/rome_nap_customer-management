// --------------------------------------------------------------------------
// <copyright file="SMATurnoverInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.7">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.TurnoverMonitor
{
    /// <summary>
    /// SMA Turnover model.
    /// </summary>
    public class SMATurnoverInfo
    {
        public int LeagueID { get; set; }

        public int MatchID { get; set; }

        public string MatchCode { get; set; }

        public int HomeID { get; set; }

        public int AwayID { get; set; }

        public int CustID { get; set; }

        public string Username { get; set; }

        public byte? RoleID { get; set; }

        public string Currency { get; set; }

        public decimal? Credit { get; set; }

        public int? Value { get; set; }

        public decimal? MaxTurnover { get; set; }

        public decimal? TotalTurnover { get; set; }

        public int? PercentageUsed { get; set; }
    }
}