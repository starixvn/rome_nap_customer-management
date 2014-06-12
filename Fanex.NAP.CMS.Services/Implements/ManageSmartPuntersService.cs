// --------------------------------------------------------------------------
// <copyright file="ManageSmartPuntersService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.4">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Implements
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Fanex.NAP.CMS.Core.Constants;
    using Fanex.NAP.CMS.Criteria.ManageSmartPunters;
    using Fanex.NAP.CMS.Models.ManageSmartPunters;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.ManageSmartPunters;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Manage smart punters service.
    /// </summary>
    public class ManageSmartPuntersService : CMSBaseService, IManageSmartPuntersService
    {
        /// <summary>
        /// Gets the smart punter list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SmartPunterItem objects.</returns>
        public IEnumerable<SmartPunterItem> GetSmartPunters(GetSmartPuntersCriteria criteria)
        {
            int totalItems = 0;
            IEnumerable<SmartPunterInfo> rawData = this.GetData<SmartPunterInfo>(criteria, out totalItems);
            IEnumerable<SmartPunterItem> result = null;

            if (rawData != null && rawData.Any())
            {
                result = rawData.Select(obj => new SmartPunterItem(obj));
                criteria.TotalItems = totalItems;
            }

            return result;
        }

        /// <summary>
        /// Adds new smart punter.
        /// </summary>
        /// <param name="filter">The filter that contains the new smart punter data.</param>
        /// <returns>UpdatingSmartPunterResult object.</returns>
        public UpdatingSmartPunterResult AddSmartPunter(AddSmartPunterFilter filter)
        {
            ICommandResult updatingResult = null;
            UpdatingSmartPunterResult smartPunters = new UpdatingSmartPunterResult();
            var getCriteria = new GetSmartPuntersCriteria
            {
                Username = filter.SearchedUsername,
                Status = filter.Status,
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize
            };
            var insertCriteria = new InsertSmartPunterCriteria
            {
                Username = filter.Username,
                Remark = filter.Remark,
                UserID = filter.UserID,
                IP = filter.IP
            };

            if (insertCriteria.IsValid())
            {
                updatingResult = this.UpdateData(insertCriteria);

                if (updatingResult.IsSuccessful)
                {
                    Thread.Sleep(CommonConstants.RepDelayTime); // Delay 3s due to the replication of DB.
                    smartPunters.SmartPunters = this.GetSmartPunters(getCriteria);
                    smartPunters.TotalItems = getCriteria.TotalItems;
                }
                else
                {
                    smartPunters.ErrorCode = updatingResult.Code;
                    smartPunters.ErrorMsg = updatingResult.Message;
                }
            }

            return smartPunters;
        }

        /// <summary>
        /// Updates an existing smart punter.
        /// </summary>
        /// <param name="filter">The filter that contains the updated data of an existing smart punter.</param>
        /// <returns>UpdatingSmartPunterResult object.</returns>
        public UpdatingSmartPunterResult UpdateSmartPunter(UpdateSmartPunterFilter filter)
        {
            ICommandResult updatingResult = null;
            UpdatingSmartPunterResult smartPunters = new UpdatingSmartPunterResult();
            var getCriteria = new GetSmartPuntersCriteria
            {
                Username = filter.SearchedUsername,
                Status = filter.Status,
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize
            };
            var updateCriteria = new UpdateSmartPunterCriteria
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
                    Thread.Sleep(CommonConstants.RepDelayTime); // Delay 3s due to the replication of DB.
                    smartPunters.SmartPunters = this.GetSmartPunters(getCriteria);
                    smartPunters.TotalItems = getCriteria.TotalItems;
                }
                else
                {
                    smartPunters.ErrorCode = updatingResult.Code;
                    smartPunters.ErrorMsg = updatingResult.Message;
                }
            }

            return smartPunters;
        }

        /// <summary>
        /// Deletes an existing smart punter.
        /// </summary>
        /// <param name="filter">The filter that contains the data of an existing smart punter.</param>
        /// <returns>UpdatingSmartPunterResult object.</returns>
        public UpdatingSmartPunterResult DeleteSmartPunter(DeleteSmartPunterFilter filter)
        {
            ICommandResult updatingResult = null;
            UpdatingSmartPunterResult smartPunters = new UpdatingSmartPunterResult();
            var getCriteria = new GetSmartPuntersCriteria
            {
                Username = filter.SearchedUsername,
                Status = filter.Status,
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize
            };
            var deleteCriteria = new DeleteSmartPunterCriteria
            {
                CustID = filter.CustID,
                UserID = filter.UserID,
                IP = filter.IP
            };

            if (deleteCriteria.IsValid())
            {
                updatingResult = this.UpdateData(deleteCriteria);

                if (updatingResult.IsSuccessful)
                {
                    Thread.Sleep(CommonConstants.RepDelayTime); // Delay 3s due to the replication of DB.
                    smartPunters.SmartPunters = this.GetSmartPunters(getCriteria);
                    smartPunters.TotalItems = getCriteria.TotalItems;
                }
                else
                {
                    smartPunters.ErrorCode = updatingResult.Code;
                    smartPunters.ErrorMsg = updatingResult.Message;
                }
            }

            return smartPunters;
        }
    }
}