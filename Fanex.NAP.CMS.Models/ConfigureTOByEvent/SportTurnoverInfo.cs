// --------------------------------------------------------------------------
// <copyright file="SportTurnoverInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.17">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.ConfigureTOByEvent
{
    /// <summary>
    /// Sport turnover information queried from database.
    /// </summary>
    public class SportTurnoverInfo
    {
        public int SportID { get; set; }

        public int SportValue { get; set; }
    }
}