// --------------------------------------------------------------------------
// <copyright file="UpdateMatchTurnoverCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.21">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ConfigureTOByEvent
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria that contain the updated data of match turnover.
    /// </summary>
    public class UpdateMatchTurnoverCriteria : CriteriaBase
    {
        public int MatchID { get; set; }

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
            return "NAP_SMATurnover_MatchControlUpd";
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