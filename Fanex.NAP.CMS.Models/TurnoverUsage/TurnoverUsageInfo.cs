// --------------------------------------------------------------------------
// <copyright file="TurnoverUsageInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.6">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.TurnoverUsage
{
    /// <summary>
    /// Turnover usage model.
    /// </summary>
    public class TurnoverUsageInfo
    {
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