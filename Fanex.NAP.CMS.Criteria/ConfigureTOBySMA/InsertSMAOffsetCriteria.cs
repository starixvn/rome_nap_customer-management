// --------------------------------------------------------------------------
// <copyright file="InsertSMAOffsetCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.24">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ConfigureTOBySMA
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Criteria contain data to insert new SMA offset exception.
    /// </summary>
    public class InsertSMAOffsetCriteria : CriteriaBase
    {
        public string SMAName { get; set; }

        public int Offset { get; set; }

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
            return "NAP_SMATurnover_SMAControlIns";
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