// --------------------------------------------------------------------------
// <copyright file="GetLeagueTurnoverCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.25">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ConfigureTOByEvent
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria that are used to get league turnover.
    /// </summary>
    public class GetLeagueTurnoverCriteria : CriteriaBase
    {
        public int SportID { get; set; }

        public string ProgramID { get; set; }

        public byte DisplayMode { get; set; }

        public string CountryCode { get; set; }

        public bool IsMainMarket { get; set; }

        public string LeagueName { get; set; }

        public bool OnlyShowException { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "NAP_SMATurnover_LeagueControlSel";
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