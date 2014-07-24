// --------------------------------------------------------------------------
// <copyright file="GetLeaguesOfSportCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.21">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.Common
{
    using System;
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria that are used to get the list of leagues belong to a specific sport.
    /// </summary>
    public class GetLeaguesOfSportCriteria : CriteriaBase
    {
        public int SportID { get; set; }

        public byte Mode { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_SMATurnover_LeagueSel";
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