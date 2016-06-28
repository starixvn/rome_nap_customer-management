// --------------------------------------------------------------------------
// <copyright file="RemoveSMATurnoverByEventFilter.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.26">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ConfigureSMATOByEvent
{
    using System;

    /// <summary>
    /// The filter that is used for the purpose of removing SMA turnover by event.
    /// </summary>
    [Serializable]
    public class RemoveSMATurnoverByEventFilter
    {
        public int MatchID { get; set; }

        public int CustID { get; set; }

        public int SportID { get; set; }

        public int LeagueID { get; set; }
    }
}