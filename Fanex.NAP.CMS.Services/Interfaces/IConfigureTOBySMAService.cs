// --------------------------------------------------------------------------
// <copyright file="IConfigureTOBySMAService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.16">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.NAP.CMS.Criteria.ConfigureTOBySMA;
    using Fanex.NAP.CMS.ViewModels.ConfigureTOBySMA;
    using Fanex.NAP.Framework.Command;

    public interface IConfigureTOBySMAService
    {
        /// <summary>
        /// Gets the SMA offset.
        /// </summary>
        /// <param name="criteria">The criteria that are used to get the maximum turnover of SMA.</param>
        /// <returns>List of SMAOffset objects.</returns>
        IEnumerable<SMAOffset> GetSMAOffset(GetSMAOffsetCriteria criteria);

        /// <summary>
        /// Updates the SMA offset.
        /// </summary>
        /// <param name="criteria">The criteria contain the data of the SMA needs to be updated.</param>
        /// <returns>ICommandResult object indicates the updating is success or not.</returns>
        ICommandResult UpdateSMAOffset(UpdateSMAOffsetCriteria criteria);

        /// <summary>
        /// Inserts the SMA offset.
        /// </summary>
        /// <param name="criteria">The criteria contain the data of the SMA needs to be inserted.</param>
        /// <returns>ICommandResult object indicates the inserting is success or not.</returns>
        ICommandResult InsertSMAOffset(InsertSMAOffsetCriteria criteria);

        /// <summary>
        /// Deleted the SMA offset.
        /// </summary>
        /// <param name="criteria">The criteria contain the data of the SMA needs to be deleted.</param>
        /// <returns>ICommandResult object indicates the deleting is success or not.</returns>
        ICommandResult DeleteSMAOffset(DeleteSMAOffsetCriteria criteria);
    }
}