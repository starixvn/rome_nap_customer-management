// --------------------------------------------------------------------------
// <copyright file="WriteCasinoApiLogCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.18">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.Criteria.OpenCloseMember
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria that are used to write log of an Casino API call.
    /// </summary>
    public class WriteCasinoApiLogCriteria : CriteriaBase
    {
        public int SystemId { get; set; }

        public int UserId { get; set; }

        public int CustId { get; set; }

        public int ActionId { get; set; }

        public string FunctionName { get; set; }

        public string TrackingValue { get; set; }

        public int ReturnCode { get; set; }

        public string ReturnValue { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "WriteLog_UserLog_CasinoAPI";
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