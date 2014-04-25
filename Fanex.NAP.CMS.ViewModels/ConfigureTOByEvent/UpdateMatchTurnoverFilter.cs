// --------------------------------------------------------------------------
// <copyright file="UpdateMatchTurnoverFilter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.21">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ConfigureTOByEvent
{
    /// <summary>
    /// The filter contains values that are used to update turnover of match and get the match list after updating.
    /// </summary>
    public class UpdateMatchTurnoverFilter
    {
        public int SportID { get; set; }

        public int LeagueID { get; set; }

        public int MatchID { get; set; }

        public int Value { get; set; }

        public string MatchCode { get; set; }

        public bool OnlyShowException { get; set; }
    }
}