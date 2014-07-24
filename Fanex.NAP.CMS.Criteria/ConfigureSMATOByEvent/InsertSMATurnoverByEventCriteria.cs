// --------------------------------------------------------------------------
// <copyright file="InsertSMATurnoverByEventCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.26">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ConfigureSMATOByEvent
{
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria that contain the data of new SMA turnover.
    /// </summary>
    public class InsertSMATurnoverByEventCriteria : CriteriaBase
    {
        public string SMAName { get; set; }

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
            return "NAP_SMATurnover_SMAControlByEventIns";
        }

        /// <summary>
        /// Determines whether this criteria is valid.
        /// </summary>
        /// <returns>Whether this criteria is valid or not.</returns>
        public override bool IsValid()
        {
            return !string.IsNullOrEmpty(this.SMAName);
        }
    }
}