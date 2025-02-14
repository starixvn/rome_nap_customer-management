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

            using (IObjectDb db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var dataset = db.ExecuteDataSet(criteria))
                {
                    if (dataset.Tables.Count > 0)
                    {
                        DataTable list = dataset.Tables[0];
                        bool hasMaster = list.Columns.Contains("master");
                        bool hasAgent = list.Columns.Contains("agent");

                        foreach (DataRow row in list.Rows)
                        {
                            var member = GetMemberInfo(row, hasMaster, hasAgent);

                            memberList.Add(member);
                        }
                    }
                }
            }

            return memberList;
        }

        public UpdateResult UpdateMembersStatus(UpdateMembersStatusCriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria);

                return new UpdateResult(
                    criteria.ErrCode,
                    criteria.ErrMsg,
                    criteria.ListCustRemove,
                    criteria.UpdatedCount);
            }
        }

        /// <summary>
        /// Gets the member information.
        /// </summary>
        /// <param name="row">The row contains the raw data.</param>
        /// <param name="hasMaster">If set to <c>true</c> [has master].</param>
        /// <param name="hasAgent">If set to <c>true</c> [has agent].</param>
        /// <returns>Member information.</returns>
        private MemberStatusInfo GetMemberInfo(DataRow row, bool hasMaster, bool hasAgent)
        {
            var member = new MemberStatusInfo
            {
                CustId = Convert.ToInt32(row["custid"]),
                Member = Convert.ToString(row["member"]),
                Closed = Convert.ToBoolean(row["closed"])
            };

            if (hasMaster)
            {
                member.Master = Convert.ToString(row["master"]);
            }

            if (hasAgent)
            {
                member.Agent = Convert.ToString(row["agent"]);
            }

            return member;
        }
    }
}