// --------------------------------------------------------------------------
// <copyright file="ManageSmartPuntersRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.24">Created</change>
// </history>

namespace Fanex.NAP.CMS.Repositories.Implements
{
    using System.Collections;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAP.CMS.Criteria.ManageSmartPunters;
    using Fanex.NAP.CMS.Repositories.Interfaces;

    /// <summary>
    /// Manage smart punters repository.
    /// </summary>
    public class ManageSmartPuntersRepository : IManageSmartPuntersRepository
    {
        /// <summary>
        /// Gets customer ID by username or nickname.
        /// </summary>
        /// <param name="criteria">The criteria that are use to query data.</param>
        /// <returns>Customer ID.</returns>
        public int GetCustIDByUsernameNickname(GetCustIDByUsernameNicknameCriteria criteria)
        {
            int custID = 0;
            Hashtable param = criteria.GetSpParams();

            using (IObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(param);
                param = db.GetOuputParams();

                if (param.ContainsKey("custid"))
                {
                    custID = Converter.ToInt(param["custid"]);
                }
            }

            return custID;
        }
    }
}