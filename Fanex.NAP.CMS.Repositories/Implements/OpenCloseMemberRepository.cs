// --------------------------------------------------------------------------
// <copyright file="OpenCloseMemberRepository.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.3">Created</change>
// </history>

namespace Fanex.NAP.CMS.Repositories.Implements
{
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.NAP.CMS.Criteria.OpenCloseMember;
    using Fanex.NAP.CMS.Models.OpenCloseMember;
    using Fanex.NAP.CMS.Repositories.Interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;

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
                using (var dataset = db.ExecuteDataSet(param))
                {
                    if (dataset.Tables.Count > 0)
                    {
                        DataTable list = dataset.Tables[0];
                        bool hasMaster = list.Columns.Contains("master");
                        bool hasAgent = list.Columns.Contains("agent");

                        foreach (DataRow row in list.Rows)
                        {
                            var member = GetMemberInfo(criteria.Username, row, hasMaster, hasAgent);

                            memberList.Add(member);
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

        public UpdateResult UpdateMembersStatus(UpdateMembersStatusCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());

                var outputs = db.GetOuputParams();

                return new UpdateResult(
                    Converter.ToInt(outputs["errcode"]),
                    Converter.ToString2(outputs["errmsg"]),
                    Converter.ToString2(outputs["listcustremove"]),
                    Converter.ToInt(outputs["updatedcount"]));
            }
        }

        /// <summary>
        /// Gets the member information.
        /// </summary>
        /// <param name="row">The row contains the raw data.</param>
        /// <param name="hasMaster">If set to <c>true</c> [has master].</param>
        /// <param name="hasAgent">If set to <c>true</c> [has agent].</param>
        /// <returns>Member information.</returns>
        private MemberStatusInfo GetMemberInfo(string searchUserNname, DataRow row, bool hasMaster, bool hasAgent)
        {
            var member = new MemberStatusInfo
            {
                CustId = Convert.ToInt32(row["custid"]),
                Member = Convert.ToString(row["member"]),
                Closed = Convert.ToBoolean(row["closed"]),
                AgentId = Convert.ToInt32(row["AgentId"]),
            };

            if (hasMaster)
            {
                member.Master = member.AgentId == -4 ? searchUserNname + "__Master" : Convert.ToString(row["master"]);
            }
            else
            {
                member.Master = searchUserNname;
            }

            if (hasAgent)
            {
                member.Agent = member.AgentId == -4 ? searchUserNname + "__Agent" : member.AgentId == -3 ? member.Master + "__Agent" : Convert.ToString(row["agent"]);
            }

            return member;
        }
    }
}