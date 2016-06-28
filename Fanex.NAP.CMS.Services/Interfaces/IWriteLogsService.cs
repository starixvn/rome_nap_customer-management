// --------------------------------------------------------------------------
// <copyright file="IWriteLogsService.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.18">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.Interfaces
{
    using Fanex.NAP.CMS.Criteria.WriteLogs;
    using Fanex.NAP.Framework.Command;

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