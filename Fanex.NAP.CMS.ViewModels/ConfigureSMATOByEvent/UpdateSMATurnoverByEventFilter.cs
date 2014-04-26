// --------------------------------------------------------------------------
// <copyright file="UpdateSMATurnoverByEventFilter.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.26">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ConfigureSMATOByEvent
{
    using System;

    /// <summary>
    /// The filter that is used for the purpose of updating an existing SMA turnover by event.
    /// </summary>
    [Serializable]
    public class UpdateSMATurnoverByEventFilter
    {
        public int MatchID { get; set; }

        public int CustID { get; set; }

        public int Value { get; set; }

        public int SportID { get; set; }

        public int LeagueID { get; set; }
    }
}