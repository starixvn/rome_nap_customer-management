// --------------------------------------------------------------------------
// <copyright file="LeagueTurnover.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.17">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ConfigureTOByEvent
{
    using System;
    using Fanex.Common;
    using Fanex.NAP.CMS.Core.Web;
    using Fanex.NAP.CMS.Models.ConfigureTOByEvent;

    /// <summary>
    /// Sport turnover.
    /// </summary>
    [Serializable]
    public class LeagueTurnover
    {
        public LeagueTurnover(LeagueTurnoverInfo rawData)
        {
            if (rawData != null)
            {
                this.SportID = rawData.SportID;
                this.LeagueID = Converter.ToInt(rawData.LeagueID);
                this.LeagueName = CacheRef.GetLeagueName(this.LeagueID);
                this.Percentage = Converter.ToInt(rawData.LeagueValue);
                this.IsException = rawData.Exception;
            }
        }

        public int SportID { get; set; }

        public int LeagueID { get; set; }

        public string LeagueName { get; set; }

        public int Percentage { get; set; }

        public bool IsException { get; set; }
    }
}