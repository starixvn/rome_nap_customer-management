// --------------------------------------------------------------------------
// <copyright file="MatchTurnoverInfo.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.17">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.ConfigureTOByEvent
{
    using System;

    /// <summary>
    /// Match turnover information got from database.
    /// </summary>
    public class MatchTurnoverInfo
    {
        public int SportID { get; set; }

        public int SportValue { get; set; }

        public int LeagueID { get; set; }

        public int LeagueValue { get; set; }

        public int? MatchID { get; set; }

        public string MatchCode { get; set; }

        public int HomeID { get; set; }

        public int AwayID { get; set; }

        public DateTime EventDate { get; set; }

        public int? MatchValue { get; set; }

        public bool Exception { get; set; }
    }
}