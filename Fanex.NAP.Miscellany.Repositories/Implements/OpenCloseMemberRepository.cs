// --------------------------------------------------------------------------
// <copyright file="OpenCloseMemberRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.3">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.Repositories.Implements
{
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAP.Miscellany.Criteria.OpenCloseMember;
    using Fanex.NAP.Miscellany.Models.OpenCloseMember;
    using Fanex.NAP.Miscellany.Repositories.Interfaces;

    /// <summary>
    /// Open/Close member repository.
    /// </summary>
    public class OpenCloseMemberRepository : IOpenCloseMemberRepository
    {
        /// <summary>
        /// Gets the members' status data.
        /// </summary>
        /// <param name="criteria">The criteria that are use to query data.</param>
        /// <returns>List of MemberStatusInfo objects.</returns>
        public IEnumerable<MemberStatusInfo> GetMembersStatusData(GetMembersStatusCriteria criteria)
        {
            IEnumerable<MemberStatusInfo> memberList = null;
            Hashtable param = criteria.GetSpParams();

            using (IObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                memberList = db.Query<MemberStatusInfo>(param);
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

            return memberList;
        }
    }
}