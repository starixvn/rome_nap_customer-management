// --------------------------------------------------------------------------
// <copyright file="MatchTurnover.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.17">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ConfigureTOByEvent
{
    using System;
    using Fanex.NAP.CMS.Core.Resources;
    using Fanex.NAP.CMS.Core.Web;
    using Fanex.NAP.CMS.Models.ConfigureTOByEvent;
    using Fanex.NAP.Core.Web;

    /// <summary>
    /// Match turnover.
    /// </summary>
    [Serializable]
    public class MatchTurnover
    {
        public MatchTurnover(MatchTurnoverInfo rawData)
        {
            if (rawData != null)
            {
                this.SportID = rawData.SportID;
                this.LeagueID = rawData.LeagueID;
                this.MatchID = rawData.MatchID;
                this.MatchCode = rawData.MatchCode;
                this.MatchName = string.Format("{0} {1} {2}", CacheRef.GetTeamName(rawData.HomeID), ViewLang.Vs, CacheRef.GetTeamName(rawData.AwayID));
                this.EventDate = rawData.EventDate.ToShortFormatedDate();
                this.Percentage = rawData.MatchValue;
                this.IsException = rawData.Exception;
            }
        }

        public int SportID { get; set; }

        public int LeagueID { get; set; }

        public int MatchID { get; set; }

        public string MatchCode { get; set; }

        public string MatchName { get; set; }

        public string EventDate { get; set; }

        public int Percentage { get; set; }

        public bool IsException { get; set; }
    }
}