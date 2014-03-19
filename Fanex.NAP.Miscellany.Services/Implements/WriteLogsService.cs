// --------------------------------------------------------------------------
// <copyright file="WriteLogsService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.18">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.Services.Implements
{
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Miscellany.Criteria.WriteLogs;
    using Fanex.NAP.Miscellany.Services.Interfaces;

    /// <summary>
    /// Open close member service.
    /// </summary>
    public class WriteLogsService : MiscellanyBaseService, IWriteLogsService
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