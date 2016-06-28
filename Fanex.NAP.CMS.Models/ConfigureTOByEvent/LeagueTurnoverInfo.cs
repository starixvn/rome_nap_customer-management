// --------------------------------------------------------------------------
// <copyright file="LeagueTurnoverInfo.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.17">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.ConfigureTOByEvent
{
    /// <summary>
    /// League turnover information got from database.
    /// </summary>
    public class LeagueTurnoverInfo
    {
        public int SportID { get; set; }

        public int SportValue { get; set; }

        public int? LeagueID { get; set; }

        public int? LeagueValue { get; set; }

        public bool Exception { get; set; }
    }
}