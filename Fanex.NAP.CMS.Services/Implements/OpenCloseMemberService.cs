// --------------------------------------------------------------------------
// <copyright file="OpenCloseMemberService.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.14">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Implements
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Fanex.NAP.CMS.Core.Constants;
    using Fanex.NAP.CMS.Criteria.OpenCloseMember;
    using Fanex.NAP.CMS.Models.OpenCloseMember;
    using Fanex.NAP.CMS.Repositories.Implements;
    using Fanex.NAP.CMS.Repositories.Interfaces;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.OpenCloseMember;

    /// <summary>
    /// Open close member service.
    /// </summary>
    public class OpenCloseMemberService : CMSBaseService, IOpenCloseMemberService
    {
        private readonly IOpenCloseMemberRepository _openCloseMemberRepository;      

        public OpenCloseMemberService() :
            this(null)
        {
        }

        public OpenCloseMemberService(IOpenCloseMemberRepository openCloseMemberRepository)
        {
            this._openCloseMemberRepository = openCloseMemberRepository ?? new OpenCloseMemberRepository();
        }

        /// <summary>
        /// Gets the members' status of a super/master/agent.
        /// </summary>
        /// <param name="filter">The filter conditions.</param>
        /// <returns>Queried result.</returns>
        public GettingMembersStatusResult GetMembersStatus(GettingMembersStatusFilter filter)
        {
            var result = new GettingMembersStatusResult();
            var criteria = new GetMembersStatusCriteria
            {
                Username = filter.Username,
                IsClosed = filter.Status,
                ItemPerPage = filter.PageSize,
                PageIndex = filter.PageIndex + 1
            };
            IEnumerable<MemberStatusInfo> memberList = this._openCloseMemberRepository.GetMembersStatusData(criteria);
            var groups = memberList.GroupBy(member => new { member.Master, member.Agent }).GroupBy(agent => agent.Key.Master);

            result.ErrorCode = criteria.ErrorCode;
            result.ErrorMsg = criteria.ErrorMsg;
            result.TotalItems = criteria.TotalItems;

            if (groups.Any())
            {
                foreach (var master in groups)
                {
                    var masterDownlineStatus = new MasterDownlineStatus
                    {
                        Username = master.Key
                    };

                    foreach (var agent in master)
                    {
                        var agentDownlineStatus = new AgentDownlineStatus
                        {
                            Username = agent.Key.Agent
                        };

                        foreach (var member in agent)
                        {
                            agentDownlineStatus.AddMember(new MemberStatus(member));
                        }

                        agentDownlineStatus.MemberStatusList = agentDownlineStatus.MemberStatusList.OrderBy(obj => obj.Closed).ToList();
                        masterDownlineStatus.AddAgent(agentDownlineStatus);
                    }

                    result.AddMaster(masterDownlineStatus);
                }
            }

            return result;
        }

        /// <summary>
        /// Updates members' status.
        /// </summary>
        /// <param name="filter">The filter contains the updated data.</param>
        /// <returns>Updated result.</returns>
        public UpdatedMembersStatusResult UpdateMembersStatus(UpdatedMembersStatusFilter filter)
        {
            var result = new UpdatedMembersStatusResult();          
            List<UpdatedMemberStatus> members = filter.MemberStatusList;          

            if (members.Count > 0)
            {
                var updateResult = UpdateStatusInDB(members, filter);

                if (updateResult.Code > 0)
                {                   
                    result.ErrorCode = updateResult.Code;
                    result.ErrorMsg = updateResult.Message;
                    result.ListCustRemove = updateResult.ListCustRemove;
                    result.UpdatedCount = updateResult.UpdatedCount;
                }
            }

            Thread.Sleep(CommonConstants.RepDelayTime); // Delay 3s due to the replication of DB.
            result.MembersStatusResult = GetUpdatedStatusList(filter);
            result.NumberOfFailedItems = 0;
            result.TotalItems = filter.MemberStatusList.Count;
            result.NumberOfUpdatedItems = result.TotalItems - result.NumberOfFailedItems;

            return result;
        }

        /// <summary>
        /// Updates the status in database.
        /// </summary>
        /// <param name="memberList">The member list.</param>
        /// <param name="filter">The filter contains .</param>
        /// <returns>UpdateResult instant indicates whether the updating process is success or not.</returns>
        private UpdateResult UpdateStatusInDB(List<UpdatedMemberStatus> memberList, UpdatedMembersStatusFilter filter)
        {
            var custIDs = string.Join(",", memberList.Select(obj => obj.CustId).ToArray());
            var criteria = new UpdateMembersStatusCriteria
            {
                CustIDs = custIDs,
                UpdateType = filter.UpdateType,
                UserID = filter.AdminId,
                IP = filter.IP
            };

            return _openCloseMemberRepository.UpdateMembersStatus(criteria);
        }

        /// <summary>
        /// Gets the updated status list.
        /// </summary>
        /// <param name="failedList">The failed list.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>Queried result.</returns>
        private GettingMembersStatusResult GetUpdatedStatusList(UpdatedMembersStatusFilter filter)
        {
            var getStatusFilter = new GettingMembersStatusFilter
            {
                Username = filter.SearchedUsername,
                Status = filter.Status,
                PageSize = filter.PageSize,
                PageIndex = filter.PageIndex
            };
            GettingMembersStatusResult statusList = this.GetMembersStatus(getStatusFilter);         

            return statusList;
        }
    }
}