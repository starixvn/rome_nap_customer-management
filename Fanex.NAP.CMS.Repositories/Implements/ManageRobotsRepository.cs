// --------------------------------------------------------------------------
// <copyright file="ManageRobotsRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.2">Created</change>
// </history>

namespace Fanex.NAP.CMS.Repositories.Implements
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAP.CMS.Criteria.ManageRobots;
    using Fanex.NAP.CMS.Models.ManageRobots;
    using Fanex.NAP.CMS.Repositories.Interfaces;

    /// <summary>
    /// Manage robot repository.
    /// </summary>
    public class ManageRobotsRepository : IManageRobotsRepository
    {
        /// <summary>
        /// Gets robot account data.
        /// </summary>
        /// <param name="criteria">The criteria that are use to query data.</param>
        /// <returns>List of RobotAccountInfo objects.</returns>
        public IEnumerable<RobotAccountInfo> GetRobotAccountData(GetRobotAccountsCriteria criteria)
        {
            IEnumerable<RobotAccountInfo> robotList;
            Hashtable param = criteria.GetSpParams();

            using (IObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                robotList = db.Query<RobotAccountInfo>(param);
                param = db.GetOuputParams();

                if (param.ContainsKey("totalitems"))
                {
                    criteria.TotalItems = Converter.ToInt(param["totalitems"]);
                }

                if (param.ContainsKey("errorcode"))
                {
                    criteria.ErrorCode = Converter.ToInt(param["errorcode"]);
                }

                if (param.ContainsKey("errormsg"))
                {
                    criteria.ErrorMsg = Converter.ToString2(param["errormsg"]);
                }
            }

            return robotList;
        }
    }
}