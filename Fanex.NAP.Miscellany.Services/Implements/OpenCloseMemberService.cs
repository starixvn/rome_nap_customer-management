// --------------------------------------------------------------------------
// <copyright file="OpenCloseMemberService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.14">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.Services.Implements
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.Miscellany.Criteria.OpenCloseMember;
    using Fanex.NAP.Miscellany.Models.OpenCloseMember;
    using Fanex.NAP.Miscellany.Repositories.Implements;
    using Fanex.NAP.Miscellany.Repositories.Interfaces;
    using Fanex.NAP.Miscellany.Services.Interfaces;
    using Fanex.NAP.Miscellany.ViewModels.OpenCloseMember;

    /// <summary>
    /// Open close member service.
    /// </summary>
    public class OpenCloseMemberService : BaseService, IOpenCloseMemberService
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
        /// Get the members' status of a super/master/agent.
        /// </summary>
        /// <param name="criteria">The filter conditions.</param>
        /// <returns>Queried result.</returns>
        public GettingMembersStatusResult GetMembersStatus(GetMembersStatusCriteria criteria)
        {
            var result = new GettingMembersStatusResult();
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
    }
}