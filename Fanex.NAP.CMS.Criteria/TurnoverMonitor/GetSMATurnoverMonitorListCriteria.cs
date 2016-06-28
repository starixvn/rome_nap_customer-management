// --------------------------------------------------------------------------
// <copyright file="GetSMATurnoverMonitorListCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.7">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.TurnoverMonitor
{
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria that are used to get SMA turnover monitor list.
    /// </summary>
    public class GetSMATurnoverMonitorListCriteria : CriteriaBase
    {
        public int SportType { get; set; }

        public int LeagueID { get; set; }

        public int UsedPercent { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_SMATurnover_MonitorList";
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