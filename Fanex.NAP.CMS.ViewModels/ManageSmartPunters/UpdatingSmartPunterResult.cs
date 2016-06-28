// --------------------------------------------------------------------------
// <copyright file="UpdatingSmartPunterResult.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.12">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ManageSmartPunters
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// View model represents the result of a getting robot accounts.
    /// </summary>
    [Serializable]
    public class UpdatingSmartPunterResult
    {
        public UpdatingSmartPunterResult()
        {
            this.SmartPunters = new List<SmartPunterItem>();
        }

        public IEnumerable<SmartPunterItem> SmartPunters { get; set; }

        public int TotalItems { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }
    }
}