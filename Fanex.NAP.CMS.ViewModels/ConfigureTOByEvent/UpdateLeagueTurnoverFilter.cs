// --------------------------------------------------------------------------
// <copyright file="UpdateLeagueTurnoverFilter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.21">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ConfigureTOByEvent
{
    /// <summary>
    /// The filter contains values that are used to update turnover of league and get the league list after updating.
    /// </summary>
    public class UpdateLeagueTurnoverFilter
    {
        public int SportID { get; set; }

        public int LeagueID { get; set; }

        public int Value { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public bool OnlyShowException { get; set; }
    }
}