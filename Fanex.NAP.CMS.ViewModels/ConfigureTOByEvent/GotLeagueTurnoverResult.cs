// --------------------------------------------------------------------------
// <copyright file="GotLeagueTurnoverResult.cs">
// Copyright (c). All rights reserved.
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
    /// View model contains the list of league turnovers and upper level data.
    /// </summary>
    [Serializable]
    public class GotLeagueTurnoverResult
    {
        public GotLeagueTurnoverResult()
        {
            this.SportName = string.Empty;
        }

        public GotLeagueTurnoverResult(int sportID, int sportValue, IEnumerable<LeagueTurnover> leagueTurnovers)
        {
            this.SportID = sportID;
            this.SportName = CacheRef.GetSportName(this.SportID);
            this.SportValue = sportValue;
            this.LeagueTurnovers = leagueTurnovers;
        }

        public int SportID { get; set; }

        public string SportName { get; set; }

        public int SportValue { get; set; }

        public IEnumerable<LeagueTurnover> LeagueTurnovers { get; set; }
    }
}