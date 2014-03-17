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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
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
            List<MemberStatusInfo> memberList = new List<MemberStatusInfo>();
            Hashtable param = criteria.GetSpParams();

            using (IObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                ////memberList = db.Query<MemberStatusInfo>(param);
                using (var dataset = db.ExecuteDataSet(param))
                {
                    if (dataset.Tables.Count > 0)
                    {
                        DataTable list = dataset.Tables[0];

                        foreach (DataRow row in list.Rows)
                        {
                            memberList.Add(new MemberStatusInfo
                            {
                                CustId = Convert.ToInt32(row["custid"]),
                                Master = Convert.ToString(row["master"]),
                                Agent = Convert.ToString(row["agent"]),
                                Member = Convert.ToString(row["member"]),
                                SyncCasino = Convert.ToBoolean(row["synccasino"]),
                                Closed = Convert.ToBoolean(row["closed"])
                            });
                        }
                    }
                }

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