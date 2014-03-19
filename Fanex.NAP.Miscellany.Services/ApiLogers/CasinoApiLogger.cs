// --------------------------------------------------------------------------
// <copyright file="CasinoApiLogger.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.19">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.Services.ApiLogers
{
    using Fanex.NAP.Miscellany.Criteria.WriteLogs;
    using Fanex.NAP.Miscellany.Services.Implements;
    using Fanex.NAP.Miscellany.Services.Interfaces;
    using Sunplus.Libraries.APILogger;

    /// <summary>
    /// Casino API Logger.
    /// </summary>
    public class CasinoApiLogger : IAPILogger
    {
        private byte _systemId;
        private GetCustId _custIdDelegate;
        private GetUserId _userIdDelegate;

        public CasinoApiLogger(byte systemId, GetUserId userIdDelegate, GetCustId custIdDelegate)
        {
            this._systemId = systemId;
            this._userIdDelegate = userIdDelegate;
            this._custIdDelegate = custIdDelegate;
        }

        public delegate int GetUserId();

        public delegate int GetCustId();

        /// <summary>
        /// Writes logs for Casino API calls.
        /// </summary>
        /// <param name="actionId">The action identifier.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="strackingValue">The tracking value.</param>
        /// <param name="returnValue">The return value.</param>
        /// <param name="returnCode">The return code.</param>
        public void Write(int actionId, string functionName, string strackingValue, string returnValue, int returnCode)
        {
            int userId = _userIdDelegate();
            int custId = _custIdDelegate();
            IWriteLogsService writeLogsService = new WriteLogsService();

            writeLogsService.WriteCasinoApiLog(new WriteCasinoApiLogCriteria
            {
                SystemID = this._systemId,
                ActionID = actionId,
                FuncName = functionName,
                CustID = custId,
                UserID = userId,
                TrackingValue = strackingValue,
                ReturnValue = returnValue,
                ReturnCode = returnCode
            });
        }
    }
}