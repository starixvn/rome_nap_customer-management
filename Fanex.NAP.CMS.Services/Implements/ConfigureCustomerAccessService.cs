// --------------------------------------------------------------------------
// <copyright file="ConfigureCustomerAccessService.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Implements
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.CMS.Criteria.ConfigureCustomerAccess;
    using Fanex.NAP.CMS.Models.ConfigureCustomerAccess;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.ConfigureCustomerAccess;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Access management service.
    /// </summary>
    public class ConfigureCustomerAccessService : CMSBaseService, IConfigureCustomerAccessService
    {
        /// <summary>
        /// Gets the request list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of RequestItem objects.</returns>
        public IEnumerable<RequestItem> GetRequestList(GetRequestInfoListCriteria criteria)
        {
            IEnumerable<RequestInfo> rawData = this.Repository.Fetch<RequestInfo>(criteria);
            IEnumerable<RequestItem> result = null;

            if (rawData != null && rawData.Any())
            {
                result = rawData.Select(obj => new RequestItem(obj, this.CountryList));
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
            RequestInfo rawData = this.Repository.Get<RequestInfo>(criteria);
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