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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using CasinoLibrary.Model.UserException;
    using CasinoLibrary.SBAgent;
    using Fanex.NAP.CMS.Core.Constants;
    using Fanex.NAP.CMS.Criteria.OpenCloseMember;
    using Fanex.NAP.CMS.Models.OpenCloseMember;
    using Fanex.NAP.CMS.Repositories.Implements;
    using Fanex.NAP.CMS.Repositories.Interfaces;
    using Fanex.NAP.CMS.Services.ApiLogers;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.OpenCloseMember;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Framework.Command;
    using Sunplus.Libraries.APILogger;

    /// <summary>
    /// Open close member service.
    /// </summary>
    public class OpenCloseMemberService : CMSBaseService, IOpenCloseMemberService
    {
        private const int BatchLimitation = 50;
        private const int AccountingSystemId = 3;

        private static int userId = 0;

        private readonly IOpenCloseMemberRepository _openCloseMemberRepository;

        static OpenCloseMemberService()
        {
            CasinoApiLogger casinoLogger = new CasinoApiLogger(AccountingSystemId, GetUserId, GetUserId);
            APILogManager apiLogManager = new APILogManager(casinoLogger);
            apiLogManager.Register(
                              typeof(CasinoLibrary.SBAgent.Customer),
                              "Update*|Insert*|AutoInsert*|Move*|Remove*|Reset*|FullSettlementResetMaxTransfer|Rollback*");
        }

        public OpenCloseMemberService() :
            this(null, null)
        {
        }

        public OpenCloseMemberService(IServiceContext serviceContext) :
            this(serviceContext, null)
        {
        }

        public OpenCloseMemberService(IServiceContext serviceContext, IOpenCloseMemberRepository openCloseMemberRepository)
        {
            this.ServiceContext = serviceContext ?? new ServiceContext();
            this._openCloseMemberRepository = openCloseMemberRepository ?? new OpenCloseMemberRepository();

            userId = this.ServiceContext.UserId;
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
            var groups = memberList.GroupBy(member => new { Master = member.Master, Agent = member.Agent }).GroupBy(agent => agent.Key.Master);

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
            List<UpdatedMemberStatus> syncCasinoMembers = filter.MemberStatusList.Where(obj => obj.SyncCasino).ToList();
            List<UpdatedMemberStatus> members = filter.MemberStatusList.Where(obj => !obj.SyncCasino).ToList();
            List<UpdatedMemberStatus> updatedRnGMembers;

            try
            {
                updatedRnGMembers = UpdateRnGCasinoStatus(syncCasinoMembers, filter);
            }
            catch (ErrorException ex)
            {
                result.ErrorCode = 100;
                result.ErrorMsg = ex.Message;
                updatedRnGMembers = new List<UpdatedMemberStatus>();
            }

            List<UpdatedMemberStatus> failedList = syncCasinoMembers.Except(updatedRnGMembers).ToList();

            if (updatedRnGMembers.Count > 0)
            {
                members.AddRange(updatedRnGMembers);
            }

            if (members.Count > 0)
            {
                ICommandResult updateResult = UpdateStatusInDB(members, filter);

                if (updateResult.Code > 0)
                {
                    failedList = filter.MemberStatusList;
                    result.ErrorCode = updateResult.Code;
                    result.ErrorMsg = updateResult.Message;
                }
            }

            Thread.Sleep(CommonConstants.RepDelayTime); // Delay 3s due to the replication of DB.
            result.MembersStatusResult = GetUpdatedStatusList(failedList, filter);
            result.NumberOfFailedItems = failedList.Count;
            result.TotalItems = filter.MemberStatusList.Count;
            result.NumberOfUpdatedItems = result.TotalItems - result.NumberOfFailedItems;

            return result;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <returns>The user ID.</returns>
        private static int GetUserId()
        {
            return userId;
        }

        /// <summary>
        /// Updates the RNG casino status.
        /// </summary>
        /// <param name="memberList">List of members that need to synchronize status with RNG Casino.</param>
        /// <param name="filter">The filter contains updated data.</param>
        /// <returns>List of members whose status is updated successfully.</returns>
        private List<UpdatedMemberStatus> UpdateRnGCasinoStatus(List<UpdatedMemberStatus> memberList, UpdatedMembersStatusFilter filter)
        {
            var updatedMembers = new List<UpdatedMemberStatus>();

            if (memberList.Count > 0)
            {
                var customerManager = new Customer();
                int numOfUpdatedMembers = memberList.Count();
                int index = 0;

                try
                {
                    while (index < numOfUpdatedMembers)
                    {
                        int count = Math.Min(BatchLimitation, numOfUpdatedMembers - index);
                        IEnumerable<UpdatedMemberStatus> tempMemberList = memberList.GetRange(index, count);
                        int[] custIdArray = tempMemberList.Select(obj => obj.CustId).ToArray();
                        string custIDList = string.Join(",", custIdArray);
                        bool updateResult = customerManager.UpdateMultipleCustClosed(
                                                                custIDList,
                                                                filter.UpdateType == UpdateStatusType.Close,
                                                                filter.IP,
                                                                filter.AdminId,
                                                                filter.AdminUsername,
                                                                AccountingSystemId);

                        if (updateResult)
                        {
                            updatedMembers.AddRange(tempMemberList);
                        }
                        else
                        {
                            break;
                        }

                        index += count;
                    }
                }
                catch (ErrorException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    // Assum that any exception occurs during calling API is a failure and CasinoApiLogger will write log for this.
                    // No need to throw exception here.
                    Logging.LogService.Debug(ex.Message);
                }
            }

            return updatedMembers;
        }

        /// <summary>
        /// Updates the status in database.
        /// </summary>
        /// <param name="memberList">The member list.</param>
        /// <param name="filter">The filter contains .</param>
        /// <returns>ICommandResult instant indicates whether the updating process is success or not.</returns>
        private ICommandResult UpdateStatusInDB(List<UpdatedMemberStatus> memberList, UpdatedMembersStatusFilter filter)
        {
            var custIDs = string.Join(",", memberList.Select(obj => obj.CustId).ToArray());
            var criteria = new UpdateMembersStatusCriteria
            {
                CustIDs = custIDs,
                UpdateType = filter.UpdateType,
                UserID = filter.AdminId,
                IP = filter.IP
            };

            return this.UpdateData(criteria);
        }

        /// <summary>
        /// Gets the updated status list.
        /// </summary>
        /// <param name="failedList">The failed list.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>Queried result.</returns>
        private GettingMembersStatusResult GetUpdatedStatusList(List<UpdatedMemberStatus> failedList, UpdatedMembersStatusFilter filter)
        {
            var getStatusFilter = new GettingMembersStatusFilter
            {
                Username = filter.SearchedUsername,
                Status = filter.Status,
                PageSize = filter.PageSize,
                PageIndex = filter.PageIndex
            };
            GettingMembersStatusResult statusList = this.GetMembersStatus(getStatusFilter);

            foreach (var master in statusList.DownlineList)
            {
                foreach (var agent in master.AgentStatusList)
                {
                    foreach (var member in agent.MemberStatusList)
                    {
                        if (failedList.Any(obj => obj.CustId == member.CustId))
                        {
                            member.IsChecked = true;
                        }
                    }
                }
            }

            return statusList;
        }
    }
}