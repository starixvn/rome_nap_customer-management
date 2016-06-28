// --------------------------------------------------------------------------
// <copyright file="GetSMATurnoverByEventCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.26">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ConfigureSMATOByEvent
{
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria that are used to get SMA turnover by event.
    /// </summary>
    public class GetSMATurnoverByEventCriteria : CriteriaBase
    {
        public int SportID { get; set; }

        public int LeagueID { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_SMATurnover_SMAControlByEventSel";
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