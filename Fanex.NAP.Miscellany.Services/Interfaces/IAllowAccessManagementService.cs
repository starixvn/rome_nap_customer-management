// --------------------------------------------------------------------------
// <copyright file="IAllowAccessManagementService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Miscellany.Criteria.AllowAccessManagement;
    using Fanex.NAP.Miscellany.ViewModels.AllowAccessManagement;

    public interface IAllowAccessManagementService
    {
        /// <summary>
        /// Gets the request list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of RequestItem objects.</returns>
        IEnumerable<RequestItem> GetRequestList(GetRequestInfoListCriteria criteria);

        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>RequestItem objects.</returns>
        RequestItem GetRequest(GetRequestInfoCriteria criteria);

        /// <summary>
        /// Inserts new request.
        /// </summary>
        /// <param name="criteria">The criteria contain data of the new request.</param>
        /// <returns>ICommandResult object indicates the inserting process is success or not.</returns>
        ICommandResult InsertRequest(InsertRequestCriteria criteria);

        /// <summary>
        /// Update an existing request.
        /// </summary>
        /// <param name="criteria">The criteria contain updated data of the request.</param>
        /// <returns>ICommandResult object indicates the updating process is success or not.</returns>
        ICommandResult UpdateRequest(UpdateRequestCriteria criteria);

        /// <summary>
        /// Delete an existing request.
        /// </summary>
        /// <param name="criteria">The criteria contain data of the request.</param>
        /// <returns>ICommandResult object indicates the process is success or not.</returns>
        ICommandResult DeleteRequest(DeleteRequestCriteria criteria);
    }
}