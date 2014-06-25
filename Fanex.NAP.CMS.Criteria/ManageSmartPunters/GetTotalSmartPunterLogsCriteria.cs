// --------------------------------------------------------------------------
// <copyright file="GetTotalSmartPunterLogsCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.25">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ManageSmartPunters
{
    using System;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get number of records that satisfy the filter conditions.
    /// </summary>
    public class GetTotalSmartPunterLogsCriteria : CriteriaBase
    {
        public GetTotalSmartPunterLogsCriteria()
        {
            this.IsInternal = 1; // For accounting site.
            this.LowerActionID = 31000; // Lowest action ID of smart punters' log.
            this.UpperActionID = 31100; // Highest action ID of smart punters' log.
            this.ActionTypeID = -1; // Default value of @ActionTypeId paramemeter.
        }

        public int CustId { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int ActionTypeID { get; set; }

        public byte IsInternal { get; set; }

        public int LowerActionID { get; set; }

        public int UpperActionID { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "Acc_UserLog_RunSPSelTotalRecords";
        }

        /// <summary>
        /// Determines whether this criteria is valid.
        /// </summary>
        /// <returns>Whether this criteria is valid or not.</returns>
        public override bool IsValid()
        {
            return this.CustId > 0;
        }
    }
}