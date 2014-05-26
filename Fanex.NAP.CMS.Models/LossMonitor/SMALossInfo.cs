// --------------------------------------------------------------------------
// <copyright file="SMALossInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.26">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.LossMonitor
{
    /// <summary>
    /// SMA loss model.
    /// </summary>
    public class SMALossInfo
    {
        public byte LevelID { get; set; }

        public int Value { get; set; }

        public string Username { get; set; }

        public byte? RoleID { get; set; }

        public string Currency { get; set; }

        public decimal? TotalBalance { get; set; }

        public decimal? MaxLoss { get; set; }

        public int? PercentageLoss { get; set; }
    }
}