// --------------------------------------------------------------------------
// <copyright file="InsertSMATurnoverByEventFilter.cs">
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
    /// The filter that is used for the purpose of inserting a new SMA turnover by event.
    /// </summary>
    [Serializable]
    public class InsertSMATurnoverByEventFilter
    {
        public int MatchID { get; set; }

        public string SMAName { get; set; }

        public int Value { get; set; }

        public int SportID { get; set; }

        public int LeagueID { get; set; }
    }
}