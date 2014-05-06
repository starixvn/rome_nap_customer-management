// --------------------------------------------------------------------------
// <copyright file="MatchInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.5">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.Common
{
    /// <summary>
    /// Model contains the general information of a match.
    /// </summary>
    public class MatchInfo
    {
        public int MatchID { get; set; }

        public int HomeID { get; set; }

        public int AwayID { get; set; }

        public string MatchName { get; set; }
    }
}