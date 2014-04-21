// --------------------------------------------------------------------------
// <copyright file="SportTurnover.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.17">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ConfigureTOByEvent
{
    using System;
    using Fanex.NAP.CMS.Core.Web;
    using Fanex.NAP.CMS.Models.ConfigureTOByEvent;

    /// <summary>
    /// Sport turnover.
    /// </summary>
    [Serializable]
    public class SportTurnover
    {
        public SportTurnover(SportTurnoverInfo rawData)
        {
            if (rawData != null)
            {
                this.SportID = rawData.SportID;
                this.SportName = CacheRef.GetSportName(this.SportID);
                this.Percentage = rawData.SportValue;
            }
        }

        public int SportID { get; set; }

        public string SportName { get; set; }

        public int Percentage { get; set; }
    }
}