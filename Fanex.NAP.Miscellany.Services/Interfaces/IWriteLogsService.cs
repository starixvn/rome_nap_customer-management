// --------------------------------------------------------------------------
// <copyright file="IWriteLogsService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.18">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.Services.Interfaces
{
    using Fanex.NAP.Framework.Command;
    using Fanex.NAP.Miscellany.Criteria.WriteLogs;

    public interface IWriteLogsService
    {
        /// <summary>
        /// Writes the casino API log.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>CommandResult object indicates whether the log is written successfully or not.</returns>
        ICommandResult WriteCasinoApiLog(WriteCasinoApiLogCriteria criteria);
    }
}