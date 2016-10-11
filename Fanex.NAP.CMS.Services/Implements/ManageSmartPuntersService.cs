// --------------------------------------------------------------------------
// <copyright file="ManageSmartPuntersService.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.4">Created</change>
// </history>

using Fanex.NAP.CMS.Core.Helpers;

namespace Fanex.NAP.CMS.Services.Implements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Xml.Linq;
    using Fanex.Common;
    using Fanex.NAP.CMS.Core.Constants;
    using Fanex.NAP.CMS.Core.Resources;
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
        private const int InvalidUsernameErrorCode = 100;

        /// <summary>
        /// Gets the smart punter list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SmartPunterItem objects.</returns>
        public IEnumerable<SmartPunterItem> GetSmartPunters(GetSmartPuntersCriteria criteria)
        {
            IEnumerable<SmartPunterInfo> rawData = this.Repository.Fetch<SmartPunterInfo>(criteria);
            IEnumerable<SmartPunterItem> result = null;

            if (rawData != null && rawData.Any())
            {
                result = rawData.Select(obj => new SmartPunterItem(obj));
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
                RoleId = filter.RoleId,
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

        /// <summary>
        /// Gets the smart punter logs.
        /// </summary>
        /// <param name="filter">The filter object that is used to get logs related to a specific smart punter.</param>
        /// <returns>List of SmartPunterLogItem items.</returns>
        public GettingSmartPunterLogsResult GetSmartPunterLogs(GetSmartPunterLogsFilter filter)
        {
            GettingSmartPunterLogsResult logResult = new GettingSmartPunterLogsResult();
            IEnumerable<SmartPunterLogInfo> rawLogs = null;
            GetCustIDByUsernameNicknameCriteria gettingCustIDCriteria = new GetCustIDByUsernameNicknameCriteria
            {
                Username = filter.Username
            };
            this.Repository.Get<int>(gettingCustIDCriteria);

            if (gettingCustIDCriteria.CustID > 0)
            {
                rawLogs = QuerySmartPunterLogs(filter, gettingCustIDCriteria.CustID);
                logResult.TotalItems = QueryTotalItems(filter, gettingCustIDCriteria.CustID);

                if (rawLogs != null && rawLogs.Any())
                {
                    SmartPunterDetailLogInfo detail = null;
                    SmartPunterDetailLogInfo oldDetail = null;
                    SmartPunterLogItem logItem = null;

                    foreach (SmartPunterLogInfo rawLog in rawLogs)
                    {
                        detail = ConvertTrackingValue(rawLog.TrackingValue);
                        oldDetail = ConvertTrackingValue(rawLog.TrackingValueOld);
                        logItem = new SmartPunterLogItem(rawLog, detail, oldDetail);
                        logResult.Logs.Add(logItem);
                    }
                }
            }
            else
            {
                logResult.ErrorCode = InvalidUsernameErrorCode;
                logResult.ErrorMsg = Messages.InvalidUsername;
            }

            return logResult;
        }

        public GetActionMonitoringLogsResult GetActionMonitoringLogs(GetActionMonitoringLogsFilter filter)
        {
            GetActionMonitoringLogsCriteria criteria = new GetActionMonitoringLogsCriteria
            {
                FromDate = filter.FromDate,
                ToDate = filter.ToDate,
                UserName = filter.UserName,
                Admin = filter.Admin
            };

            IEnumerable<ActionMonitoringLog> actionMonitoringLogs = this.Repository.Fetch<ActionMonitoringLog>(criteria).ToList();
            int totalItems = actionMonitoringLogs.Count();

            actionMonitoringLogs = SortActionMonitoringLogs(actionMonitoringLogs, filter.SortDirection, filter.SortField);

            actionMonitoringLogs = actionMonitoringLogs.Skip(filter.PageSize * filter.PageIndex).Take(filter.PageSize).ToList();

            GetActionMonitoringLogsResult result = new GetActionMonitoringLogsResult
            {
                ActionMonitoringLogs = actionMonitoringLogs.ToList(),
                TotalItems = totalItems
            };

            return result;
        }

        private IEnumerable<ActionMonitoringLog> SortActionMonitoringLogs(IEnumerable<ActionMonitoringLog> actionMonitoringLogs, string direction, string propertyName)
        {
            if (propertyName == nameof(ActionMonitoringLog.RoleName))
            {
                propertyName = nameof(ActionMonitoringLog.RoleId);
            }

            var result = (direction == "asc" ? actionMonitoringLogs.OrderBy(propertyName) : actionMonitoringLogs.OrderByDescending(propertyName));

            return result;
        }

        /// <summary>
        /// Queries the number of smart punters that satisfy the filter.
        /// </summary>
        /// <param name="filter">The filter conditions.</param>
        /// <param name="custID">The customer identifier.</param>
        /// <returns>Number of smart punters that satisfy the filter.</returns>
        private int QueryTotalItems(GetSmartPunterLogsFilter filter, int custID)
        {
            var criteria = new GetTotalSmartPunterLogsCriteria
            {
                CustId = custID,
                FromDate = filter.FromDate,
                ToDate = filter.ToDate
            };
            int totalItems = this.Repository.Get<int>(criteria);

            return totalItems;
        }

        /// <summary>
        /// Queries the smart punter logs.
        /// </summary>
        /// <param name="filter">The filter conditions.</param>
        /// <param name="custID">The customer identifier.</param>
        /// <returns>List of SmartPunterLogItem items.</returns>
        private IEnumerable<SmartPunterLogInfo> QuerySmartPunterLogs(GetSmartPunterLogsFilter filter, int custID)
        {
            var criteria = new GetSmartPunterLogsCriteria
            {
                CustId = custID,
                FromDate = filter.FromDate,
                ToDate = filter.ToDate,
                PageIndex = filter.PageIndex,
                PageSize = filter.PageSize
            };
            IEnumerable<SmartPunterLogInfo> rawLogs = this.Repository.Fetch<SmartPunterLogInfo>(criteria);

            return rawLogs;
        }

        /// <summary>
        /// Converts the tracking value into SmartPunterDetailLogInfo object.
        /// </summary>
        /// <param name="trackingValue">The tracking value.</param>
        /// <returns>SmartPunterDetailLogInfo object.</returns>
        private SmartPunterDetailLogInfo ConvertTrackingValue(string trackingValue)
        {
            SmartPunterDetailLogInfo detail = null;

            if (!string.IsNullOrEmpty(trackingValue))
            {
                XElement trackingElement = XElement.Parse(trackingValue);

                if (trackingElement.HasAttributes)
                {
                    IEnumerable<XAttribute> attributes = trackingElement.Attributes();

                    detail = new SmartPunterDetailLogInfo();
                    foreach (XAttribute attribute in attributes)
                    {
                        switch (attribute.Name.LocalName)
                        {
                            case "CustID":
                                detail.CustID = Converter.ToInt(attribute.Value);
                                break;

                            case "Remark":
                                detail.Remark = attribute.Value;
                                break;

                            case "ErrCode":
                                detail.ErrCode = !string.IsNullOrEmpty(attribute.Value) ? Converter.ToInt(attribute.Value) : detail.ErrCode;
                                break;

                            case "ErrMsg":
                                detail.ErrMsg = attribute.Value;
                                break;

                            case "IP":
                                detail.IP = attribute.Value;
                                break;
                        }
                    }
                }
            }

            return detail;
        }
    }
}