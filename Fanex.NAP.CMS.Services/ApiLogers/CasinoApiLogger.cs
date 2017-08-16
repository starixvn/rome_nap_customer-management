// --------------------------------------------------------------------------
// <copyright file="CasinoApiLogger.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.19">Created</change>
// </history>

namespace Fanex.NAP.CMS.Services.ApiLogers
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Xml;
    using Fanex.NAP.CMS.Criteria.WriteLogs;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Sunplus.Libraries.APILogger;

    public delegate int GetUserId();

    public delegate int GetCustId();

    /// <summary>
    /// Casino API Logger.
    /// </summary>
    public class CasinoApiLogger : IAPILogger
    {
        private readonly byte _systemId;
        private readonly GetUserId _userIdDelegate;

        public CasinoApiLogger(byte systemId, GetUserId userIdDelegate, GetCustId custIdDelegate)
        {
            this._systemId = systemId;
            this._userIdDelegate = userIdDelegate;
        }

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
            var custId = string.IsNullOrWhiteSpace(HttpContext.Current.Request["id"]) ? HttpContext.Current.Request["custid"] : string.Empty;
            var urlRef = HttpContext.Current.Request.UrlReferrer;
            if (string.IsNullOrWhiteSpace(custId) || custId == "0")
            {
                var qryCustId = urlRef.Query.Replace("?", string.Empty).Split('&');                
                var item = qryCustId.FirstOrDefault(qry => qry.ToLowerInvariant().Contains("custid"));

                if (!string.IsNullOrWhiteSpace(item))
                {
                    custId = item.Split('=')[1];
                }

                if (string.IsNullOrWhiteSpace(custId) || custId == "0")
                {
                    var tracking = new XmlDocument();
                    tracking.LoadXml(strackingValue);
                    try
                    {
                        var id = tracking.GetElementsByTagName("DocumentElement").Item(0).FirstChild;
                        var nodeId = id.SelectSingleNode("intIBCCustID") ?? id.SelectSingleNode("IBCCustID");                        
                        nodeId = nodeId ?? id.SelectSingleNode("IBCCustIDList");
                        custId = nodeId.FirstChild.Value;
                    }
                    catch
                    {
                        // Ignored
                    }
                }
            }

            WriteLog(actionId, functionName, strackingValue, returnValue, returnCode, custId);
        }

        private void WriteLog(int actionId, string functionName, string strackingValue, string returnValue, int returnCode, string customerIds)
        {
            IWriteLogsService writeLogsService = new WriteLogsService();
            string custId = string.IsNullOrWhiteSpace(customerIds) ? "0" : customerIds;
            var custIds = custId.Split(',');
            foreach (var id in custIds)
            {
                writeLogsService.WriteCasinoApiLog(new WriteCasinoApiLogCriteria
                {
                    SystemID = _systemId,
                    ActionID = actionId,
                    FuncName = functionName,
                    CustID = Convert.ToInt32(id),
                    UserID = _userIdDelegate(),
                    TrackingValue = strackingValue,
                    ReturnValue = returnValue,
                    ReturnCode = returnCode
                });
            }
        }
    }
}