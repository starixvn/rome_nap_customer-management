// --------------------------------------------------------------------------
// <copyright file="ManageRobotsService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.30">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Implements
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.CMS.Criteria.ManageRobots;
    using Fanex.NAP.CMS.Models.ManageRobots;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.ManageRobots;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Manage robot service.
    /// </summary>
    public class ManageRobotsService : CMSBaseService, IManageRobotsService
    {
        /// <summary>
        /// Gets the robot accounts.
        /// </summary>
        /// <param name="criteria">The criteria that are used to get data.</param>
        /// <returns>GettingRobotAccountResult objects.</returns>
        public GettingRobotAccountResult GetRobotAccounts(GetRobotAccountsCriteria criteria)
        {
            IEnumerable<RobotAccountInfo> rawData = this.Repository.Fetch<RobotAccountInfo>(criteria);
            GettingRobotAccountResult result = new GettingRobotAccountResult
            {
                TotalItems = criteria.TotalItems,
                ErrorCode = criteria.ErrorCode,
                ErrorMsg = criteria.ErrorMsg
            };

            if (rawData != null && rawData.Any())
            {
                result.RobotAccounts = rawData.Select(obj => new RobotAccountItem(obj)).ToList();
            }

            return result;
        }

        /// <summary>
        /// Updates the robot information.
        /// </summary>
        /// <param name="filter">The filter that contains the updated data.</param>
        /// <returns>GettingRobotAccountResult object.</returns>
        public GettingRobotAccountResult UpdateRobotInfo(UpdateRobotInfoFilter filter)
        {
            ICommandResult updatingResult = null;
            GettingRobotAccountResult robotList = new GettingRobotAccountResult();
            var getCriteria = new GetRobotAccountsCriteria
            {
                Username = filter.SearchedUsername,
                Status = filter.Status,
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize
            };
            var updateCriteria = new UpdateRobotInfoCriteria
            {
                CustID = filter.CustID,
                Remark = filter.Remark,
                UserID = filter.UserID,
                IP = filter.IP
            };

            if (updateCriteria.IsValid())
            {
                updatingResult = this.UpdateData(updateCriteria);

                if (updatingResult.IsSuccessful)
                {
                    robotList = this.GetRobotAccounts(getCriteria);
                }
                else
                {
                    robotList.ErrorCode = updatingResult.Code;
                    robotList.ErrorMsg = updatingResult.Message;
                }
            }

            return robotList;
        }

        /// <summary>
        /// Adds new robot information.
        /// </summary>
        /// <param name="filter">The filter that contains the new robot data.</param>
        /// <returns>GettingRobotAccountResult object.</returns>
        public GettingRobotAccountResult AddRobotAccount(AddRobotInfoFilter filter)
        {
            ICommandResult updatingResult = null;
            GettingRobotAccountResult robotList = new GettingRobotAccountResult();
            var getCriteria = new GetRobotAccountsCriteria
            {
                Username = filter.SearchedUsername,
                Status = filter.Status,
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize
            };
            var insertCriteria = new InsertRobotAccountCriteria
            {
                Usernames = filter.Usernames,
                Status = true,
                Remark = filter.Remark,
                UserID = filter.UserID,
                IP = filter.IP
            };

            if (insertCriteria.IsValid())
            {
                updatingResult = this.UpdateData(insertCriteria);

                if (updatingResult.IsSuccessful)
                {
                    robotList = this.GetRobotAccounts(getCriteria);
                }
                else
                {
                    robotList.ErrorCode = updatingResult.Code;
                    robotList.ErrorMsg = updatingResult.Message;
                }
            }

            return robotList;
        }

        /// <summary>
        /// Updates robot status.
        /// </summary>
        /// <param name="filter">The filter that contains the robot status.</param>
        /// <returns>GettingRobotAccountResult object.</returns>
        public GettingRobotAccountResult UpdateRobotStatus(UpdateRobotStatusFilter filter)
        {
            ICommandResult updatingResult = null;
            GettingRobotAccountResult robotList = new GettingRobotAccountResult();
            var getCriteria = new GetRobotAccountsCriteria
            {
                Username = filter.SearchedUsername,
                Status = filter.Status,
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize
            };
            var updateCriteria = new UpdateRobotStatusCriteria
            {
                CustIDs = filter.CustIDs,
                Status = filter.IsActive,
                UserID = filter.UserID,
                IP = filter.IP
            };

            if (updateCriteria.IsValid())
            {
                updatingResult = this.UpdateData(updateCriteria);

                if (updatingResult.IsSuccessful)
                {
                    robotList = this.GetRobotAccounts(getCriteria);
                }
                else
                {
                    robotList.ErrorCode = updatingResult.Code;
                    robotList.ErrorMsg = updatingResult.Message;
                }
            }

            return robotList;
        }
    }
}