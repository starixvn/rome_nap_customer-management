// --------------------------------------------------------------------------
// <copyright file="RemoveMatchExceptionCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.21">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ConfigureTOByEvent
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    /// <summary>
    /// The criteria that are used to remove a match exception.
    /// </summary>
    public class RemoveMatchExceptionCriteria : CommandBaseCriteria
    {
        public int MatchID { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_SMATurnover_MatchControlDel";
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