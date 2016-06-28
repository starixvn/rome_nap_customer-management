// --------------------------------------------------------------------------
// <copyright file="IManageSmartPuntersService.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.4">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.NAP.CMS.Criteria.ManageSmartPunters;
    using Fanex.NAP.CMS.ViewModels.ManageSmartPunters;

    public interface IManageSmartPuntersService
    {
        /// <summary>
        /// Gets the smart punter list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of SmartPunterItem objects.</returns>
        IEnumerable<SmartPunterItem> GetSmartPunters(GetSmartPuntersCriteria criteria);

        /// <summary>
        /// Adds new smart punter.
        /// </summary>
        /// <param name="filter">The filter that contains the new smart punter data.</param>
        /// <returns>UpdatingSmartPunterResult object.</returns>
        UpdatingSmartPunterResult AddSmartPunter(AddSmartPunterFilter filter);

        /// <summary>
        /// Updates an existing smart punter.
        /// </summary>
        /// <param name="filter">The filter that contains the updated data of an existing smart punter.</param>
        /// <returns>UpdatingSmartPunterResult object.</returns>
        UpdatingSmartPunterResult UpdateSmartPunter(UpdateSmartPunterFilter filter);

        /// <summary>
        /// Deletes an existing smart punter.
        /// </summary>
        /// <param name="filter">The filter that contains the data of an existing smart punter.</param>
        /// <returns>UpdatingSmartPunterResult object.</returns>
        UpdatingSmartPunterResult DeleteSmartPunter(DeleteSmartPunterFilter filter);

        /// <summary>
        /// Gets the smart punter logs.
        /// </summary>
        /// <param name="filter">The filter object that is used to get logs related to a specific smart punter.</param>
        /// <returns>List of SmartPunterLogItem items.</returns>
        GettingSmartPunterLogsResult GetSmartPunterLogs(GetSmartPunterLogsFilter filter);
    }
}