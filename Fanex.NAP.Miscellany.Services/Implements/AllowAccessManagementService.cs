// --------------------------------------------------------------------------
// <copyright file="AllowAccessManagementService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.Services.Implements
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Miscellany.Criteria.AllowAccessManagement;
    using Fanex.NAP.Miscellany.Models.AllowAccessManagement;
    using Fanex.NAP.Miscellany.Services.Interfaces;
    using Fanex.NAP.Miscellany.ViewModels.AllowAccessManagement;

    /// <summary>
    /// Access management service.
    /// </summary>
    public class AllowAccessManagementService : BaseService, IAllowAccessManagementService
    {
        /// <summary>
        /// Gets the request list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of RequestItem objects.</returns>
        public IEnumerable<RequestItem> GetRequestList(GetRequestInfoListCriteria criteria)
        {
            int totalItems = 0;
            IEnumerable<RequestInfo> rawData = this.GetData<RequestInfo>(criteria, out totalItems);
            IEnumerable<RequestItem> result = null;

            if (rawData != null && rawData.Any())
            {
                result = rawData.Select(obj => new RequestItem(obj, this.CountryList));
                criteria.TotalItems = totalItems;
            }

            return result;
        }

        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>RequestItem objects.</returns>
        public RequestItem GetRequest(GetRequestInfoCriteria criteria)
        {
            RequestInfo rawData = this.GetItem<RequestInfo>(criteria);
            RequestItem result = null;

            if (rawData != null)
            {
                result = new RequestItem(rawData, this.CountryList);
            }

            return result;
        }

        /// <summary>
        /// Inserts new request.
        /// </summary>
        /// <param name="criteria">The criteria contain data of the new request.</param>
        /// <returns>ICommandResult object indicates the inserting process is success or not.</returns>
        public ICommandResult InsertRequest(InsertRequestCriteria criteria)
        {
            return this.UpdateData(criteria);
        }

        /// <summary>
        /// Update an existing request.
        /// </summary>
        /// <param name="criteria">The criteria contain updated data of the request.</param>
        /// <returns>ICommandResult object indicates the updating process is success or not.</returns>
        public ICommandResult UpdateRequest(UpdateRequestCriteria criteria)
        {
            return this.UpdateData(criteria);
        }

        /// <summary>
        /// Delete an existing request.
        /// </summary>
        /// <param name="criteria">The criteria contain data of the request.</param>
        /// <returns>ICommandResult object indicates the process is success or not.</returns>
        public ICommandResult DeleteRequest(DeleteRequestCriteria criteria)
        {
            return this.UpdateData(criteria);
        }
    }
}