// --------------------------------------------------------------------------
// <copyright file="SMATurnoverReport.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.28">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.TurnoverMonitor
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// View model contains live and non-live data.
    /// </summary>
    [Serializable]
    public class SMATurnoverReport
    {
        public IEnumerable<SMATurnoverGroup> Live { get; set; }

        public IEnumerable<SMATurnoverGroup> NonLive { get; set; }
    }
}