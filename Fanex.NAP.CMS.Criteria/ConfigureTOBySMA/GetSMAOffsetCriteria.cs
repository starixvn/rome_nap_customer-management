// --------------------------------------------------------------------------
// <copyright file="GetSMAOffsetCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.24">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ConfigureTOBySMA
{
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria that are used to get SMA turnover.
    /// </summary>
    public class GetSMAOffsetCriteria : CriteriaBase
    {
        public string SMAName { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_SMATurnover_SMAControlSel";
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