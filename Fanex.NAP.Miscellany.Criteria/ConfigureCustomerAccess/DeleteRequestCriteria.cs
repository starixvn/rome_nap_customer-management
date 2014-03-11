// --------------------------------------------------------------------------
// <copyright file="DeleteRequestCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.Criteria.ConfigureCustomerAccess
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// The criteria that are used to delete a request.
    /// </summary>
    public class DeleteRequestCriteria : CriteriaBase
    {
        public int RequestID { get; set; }

        public int DeletedBy { get; set; }

        public string IPAction { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        /// <summary>
        /// Gets the setting key.
        /// </summary>
        /// <returns>The setting key.</returns>
        public override string GetSettingKey()
        {
            return "Member_LoginVerifyDel";
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