// --------------------------------------------------------------------------
// <copyright file="WriteLogsService.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.18">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Implements
{
    using Fanex.NAP.CMS.Criteria.WriteLogs;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Open close member service.
    /// </summary>
    public class WriteLogsService : CMSBaseService, IWriteLogsService
    {
        /// <summary>
        /// Writes the casino API log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>CommandResult object indicates whether the log is written successfully or not.</returns>
        public ICommandResult WriteCasinoApiLog(WriteCasinoApiLogCriteria criteria)
        {
            return this.UpdateData(criteria);
        }
    }
}