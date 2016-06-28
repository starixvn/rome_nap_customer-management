// --------------------------------------------------------------------------
// <copyright file="SpecialLeagueInfo.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.25">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.ConfigureTOByEvent
{
    /// <summary>
    /// Model contains the information of special league.
    /// </summary>
    public class SpecialLeagueInfo
    {
        public byte DisplayMode { get; set; }

        public string ProgramID { get; set; }

        public string DisplayName { get; set; }
    }
}