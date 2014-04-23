// --------------------------------------------------------------------------
// <copyright file="GetEventTurnoverCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.17">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ConfigureTOByEvent
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria that are used to get turnover of sport, league or match.
    /// </summary>
    public class GetEventTurnoverCriteria : CriteriaBase
    {
        public GetEventTurnoverCriteria()
        {
            this.MatchCode = string.Empty;
        }

        public int SportID { get; set; }

        public int LeagueID { get; set; }

        public string MatchCode { get; set; }

        public bool OnlyShowException { get; set; }

        public int ViewMode { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_SMATurnover_EventSel";
        }

        /// <summary>
        /// Determines whether this criteria is valid.
        /// </summary>
        /// <returns>Whether this criteria is valid or not.</returns>
        public override bool IsValid()
        {
            return true;
        }
    }
}