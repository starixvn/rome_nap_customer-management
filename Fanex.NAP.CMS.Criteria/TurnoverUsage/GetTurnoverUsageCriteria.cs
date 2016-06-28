// --------------------------------------------------------------------------
// <copyright file="GetTurnoverUsageCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.6">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.TurnoverUsage
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria that are used to get turnover usage.
    /// </summary>
    public class GetTurnoverUsageCriteria : CriteriaBase
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int SportType { get; set; }

        public int LeagueID { get; set; }

        public int MatchID { get; set; }

        public int UsedPercent { get; set; }

        public string SMAName { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_SMATurnover_UsageList";
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