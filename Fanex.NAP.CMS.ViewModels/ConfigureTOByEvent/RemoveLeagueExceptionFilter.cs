// --------------------------------------------------------------------------
// <copyright file="RemoveLeagueExceptionFilter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
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

        public int UserID { get; set; }

        public string IP { get; set; }

        public bool OnlyShowException { get; set; }
    }
}