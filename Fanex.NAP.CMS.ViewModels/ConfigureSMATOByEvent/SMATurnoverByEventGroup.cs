// --------------------------------------------------------------------------
// <copyright file="SMATurnoverByEventGroup.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.26">Created</change>
// </history>

using Fanex.NAP.CMS.Core.Helpers;

namespace Fanex.NAP.CMS.ViewModels.ConfigureSMATOByEvent
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.CMS.Core.Resources;
    using Fanex.NAP.CMS.Core.Web;
    using Fanex.NAP.CMS.Models.ConfigureSMATOByEvent;

    /// <summary>
    /// Group of SMA turnover by event view model.
    /// </summary>
    [Serializable]
    public class SMATurnoverByEventGroup
    {
        public SMATurnoverByEventGroup(SMATurnoverByEventInfo rawData)
        {
            if (rawData != null)
            {
                this.MatchID = rawData.MatchID;
                this.MatchCode = rawData.MatchCode;
                this.EventDate = rawData.EventDate.ToShortFormatedDate();
                this.MatchName = string.Format("{0} {1} {2}", CacheRef.GetTeamName(rawData.HomeID), ViewLang.Vs, CacheRef.GetTeamName(rawData.AwayID));
                this.SMAItems = new List<SMATurnoverByEvent>();
            }
        }

        public int MatchID { get; set; }

        public string MatchCode { get; set; }

        public string EventDate { get; set; }

        public string MatchName { get; set; }

        public List<SMATurnoverByEvent> SMAItems { get; set; }
    }
}