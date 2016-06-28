// --------------------------------------------------------------------------
// <copyright file="RemoveLeagueExceptionFilter.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.21">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ConfigureTOByEvent
{
    /// <summary>
    /// The filter that is used to remove a league exception and get updated list.
    /// </summary>
    public class RemoveLeagueExceptionFilter
    {
        public int SportID { get; set; }

        public int LeagueID { get; set; }

        public string ProgramID { get; set; }

        public byte DisplayMode { get; set; }

        public string CountryCode { get; set; }

        public bool AllMarket { get; set; }

        public string LeagueName { get; set; }

        public bool OnlyShowException { get; set; }
    }
}