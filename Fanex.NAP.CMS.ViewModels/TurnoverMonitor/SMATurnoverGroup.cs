// --------------------------------------------------------------------------
// <copyright file="SMATurnoverGroup.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.28">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.TurnoverMonitor
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.CMS.Core.Resources;
    using Fanex.NAP.CMS.Core.Web;
    using Fanex.NAP.CMS.Models.TurnoverMonitor;

    /// <summary>
    /// SMA turnover group view model.
    /// </summary>
    [Serializable]
    public class SMATurnoverGroup
    {
        public SMATurnoverGroup(SMATurnoverInfo rawData)
        {
            if (rawData != null)
            {
                this.LeagueName = CacheRef.GetLeagueName(rawData.LeagueID);
                this.MatchCode = rawData.MatchCode;
                this.MatchName = string.Format("{0} {1} {2}", CacheRef.GetTeamName(rawData.HomeID), ViewLang.Vs, CacheRef.GetTeamName(rawData.AwayID));
                this.SMAItems = new List<SMATurnoverItem>();
            }
        }

        public string LeagueName { get; set; }

        public string MatchCode { get; set; }

        public string MatchName { get; set; }

        public List<SMATurnoverItem> SMAItems { get; set; }
    }
}