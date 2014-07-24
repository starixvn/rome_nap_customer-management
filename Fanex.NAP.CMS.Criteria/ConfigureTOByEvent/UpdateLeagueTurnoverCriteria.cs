// --------------------------------------------------------------------------
// <copyright file="UpdateLeagueTurnoverCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.21">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ConfigureTOByEvent
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Criteria that contain the updated data of league turnover.
    /// </summary>
    public class UpdateLeagueTurnoverCriteria : CriteriaBase
    {
        public int LeagueID { get; set; }

        public int Value { get; set; }

        public int UserID { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

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