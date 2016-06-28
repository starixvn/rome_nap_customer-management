// --------------------------------------------------------------------------
// <copyright file="RemoveMatchExceptionFilter.cs">
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
    public class RemoveMatchExceptionFilter
    {
        public int SportID { get; set; }

        public int LeagueID { get; set; }

        public int MatchID { get; set; }

        public string MatchCode { get; set; }

        public bool OnlyShowException { get; set; }
    }
}