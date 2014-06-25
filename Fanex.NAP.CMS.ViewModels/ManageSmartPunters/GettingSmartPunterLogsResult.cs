// --------------------------------------------------------------------------
// <copyright file="GettingSmartPunterLogsResult.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.23">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ManageSmartPunters
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Smart punter's log result view model.
    /// </summary>
    [Serializable]
    public class GettingSmartPunterLogsResult
    {
        public GettingSmartPunterLogsResult()
        {
            this.Logs = new List<SmartPunterLogItem>();
        }

        public List<SmartPunterLogItem> Logs { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }
    }
}