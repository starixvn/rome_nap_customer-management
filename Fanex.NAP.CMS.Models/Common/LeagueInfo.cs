// --------------------------------------------------------------------------
// <copyright file="LeagueInfo.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.21">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.Common
{
    using System;

    /// <summary>
    /// The model using to save league information.
    /// </summary>
    [Serializable]
    public class LeagueInfo
    {
        public int LeagueID { get; set; }

        public string LeagueName { get; set; }
    }
}