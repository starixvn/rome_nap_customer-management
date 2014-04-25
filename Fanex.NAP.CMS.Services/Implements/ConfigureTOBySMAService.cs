// --------------------------------------------------------------------------
// <copyright file="ConfigureTOBySMAService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.16">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Implements
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.NAP.CMS.Criteria.ConfigureTOBySMA;
    using Fanex.NAP.CMS.Models.ConfigureTOBySMA;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.ConfigureTOBySMA;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Configure turnover by SMA service.
    /// </summary>
    public class ConfigureTOBySMAService : CMSBaseService, IConfigureTOBySMAService
    {
        /// <summary>
        /// Gets the SMA offset.
        /// </summary>
        /// <param name="criteria">The criteria that are used to get the maximum turnover of SMA.</param>
        /// <returns>List of SMAOffset objects.</returns>
        public IEnumerable<SMAOffset> GetSMAOffset(GetSMAOffsetCriteria criteria)
        {
            IEnumerable<SMAOffsetInfo> rawData = this.GetData<SMAOffsetInfo>(criteria);
            IEnumerable<SMAOffset> offset = null;

            if (rawData.Any())
            {
                offset = rawData.Select(obj => new SMAOffset(obj));
            }

            return offset;
        }

        /// <summary>
        /// Updates the SMA offset.
        /// </summary>
        /// <param name="criteria">The criteria contain the data of the SMA needs to be updated.</param>
        /// <returns>ICommandResult object indicates the updating is success or not.</returns>
        public ICommandResult UpdateSMAOffset(UpdateSMAOffsetCriteria criteria)
        {
            return this.UpdateData(criteria);
        }

        /// <summary>
        /// Inserts the SMA offset.
        /// </summary>
        /// <param name="criteria">The criteria contain the data of the SMA needs to be inserted.</param>
        /// <returns>ICommandResult object indicates the inserting is success or not.</returns>
        public ICommandResult InsertSMAOffset(InsertSMAOffsetCriteria criteria)
        {
            return this.UpdateData(criteria);
        }

        /// <summary>
        /// Deleted the SMA offset.
        /// </summary>
        /// <param name="criteria">The criteria contain the data of the SMA needs to be deleted.</param>
        /// <returns>ICommandResult object indicates the deleting is success or not.</returns>
        public ICommandResult DeleteSMAOffset(DeleteSMAOffsetCriteria criteria)
        {
            return this.UpdateData(criteria);
        }
    }
}