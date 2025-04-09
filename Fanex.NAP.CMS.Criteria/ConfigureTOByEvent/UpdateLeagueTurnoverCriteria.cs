// --------------------------------------------------------------------------
// <copyright file="UpdateLeagueTurnoverCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.21">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ConfigureTOByEvent
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    /// <summary>
    /// Criteria that contain the updated data of league turnover.
    /// </summary>
    public class UpdateLeagueTurnoverCriteria : CommandBaseCriteria
    {
        public int LeagueID { get; set; }

        public int Value { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_SMATurnover_LeagueControlUpd";
        }

        /// <summary>
        /// Determines whether this criteria is valid.
        /// </summary>
        /// <returns>Whether this criteria is valid or not.</returns>
        public override bool IsValid()
        {
            return Value >= 0;
        }
    }
}