// --------------------------------------------------------------------------
// <copyright file="UpdateRobotInfoCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.2">Created</change>
// </history>

namespace Fanex.NAP.CMS.Criteria.ManageRobots
{
    using Fanex.Data.Repository;

    /// <summary>
    /// The criteria that contain the updated data of a robot account.
    /// </summary>
    public class UpdateRobotInfoCriteria : CriteriaBase
    {
        public int CustID { get; set; }

        public string Remark { get; set; }

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
            return "NAP_CustomerUpdInfoRobotAccount";
        }

        /// <summary>
        /// Determines whether this criteria is valid.
        /// </summary>
        /// <returns>Whether this criteria is valid or not.</returns>
        public override bool IsValid()
        {
            return CustID > 0 && UserID > 0 && !string.IsNullOrEmpty(IP);
        }
    }
}