// --------------------------------------------------------------------------
// <copyright file="GettingRobotAccountResult.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.2">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ManageRobots
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// View model represents the result of a getting robot accounts.
    /// </summary>
    [Serializable]
    public class GettingRobotAccountResult
    {
        public GettingRobotAccountResult()
        {
            this.RobotAccounts = new List<RobotAccountItem>();
        }

        public List<RobotAccountItem> RobotAccounts { get; set; }

        public int TotalItems { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }
    }
}