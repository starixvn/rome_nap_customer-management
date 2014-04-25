// --------------------------------------------------------------------------
// <copyright file="GotMatchTurnoverResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.17">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ConfigureTOByEvent
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.CMS.Core.Web;

    /// <summary>
    /// View model contains the list of match turnovers and upper level data.
    /// </summary>
    [Serializable]
    public class GotMatchTurnoverResult
    {
        public GotMatchTurnoverResult()
        {
            this.SportName = string.Empty;
            this.LeagueName = string.Empty;
        }

        public GotMatchTurnoverResult(int sportID, int sportValue, int leagueID, int leagueValue, IEnumerable<MatchTurnover> leagueTurnovers)
        {
            this.SportID = sportID;
            this.SportName = CacheRef.GetSportName(this.SportID);
            this.SportValue = sportValue;
            this.LeagueID = leagueID;
            this.LeagueName = CacheRef.GetLeagueName(this.LeagueID);
            this.LeagueValue = leagueValue;
            this.MatchTurnovers = leagueTurnovers;
        }

        public int LeagueID { get; set; }

        public string LeagueName { get; set; }

        public int LeagueValue { get; set; }

        public int SportID { get; set; }

        public string SportName { get; set; }

        public int SportValue { get; set; }

        public IEnumerable<MatchTurnover> MatchTurnovers { get; set; }
    }
}