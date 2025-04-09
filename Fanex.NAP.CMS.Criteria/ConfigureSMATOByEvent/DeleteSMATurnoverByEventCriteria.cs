// --------------------------------------------------------------------------
// <copyright file="DeleteSMATurnoverByEventCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.26">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ConfigureSMATOByEvent
{
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    /// <summary>
    /// The criteria using to delete a SMA turnover.
    /// </summary>
    public class DeleteSMATurnoverByEventCriteria : CommandBaseCriteria
    {
        public int SMAID { get; set; }

        public int MatchID { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_SMATurnover_SMAControlByEventDel";
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