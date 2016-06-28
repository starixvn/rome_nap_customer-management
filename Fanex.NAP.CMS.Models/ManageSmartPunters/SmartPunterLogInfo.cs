// --------------------------------------------------------------------------
// <copyright file="SmartPunterLogInfo.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.23">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.ManageSmartPunters
{
    using System;

    /// <summary>
    /// Smart punter log model.
    /// </summary>
    public class SmartPunterLogInfo
    {
        public long LogID { get; set; }

        public int? UserID { get; set; }

        public string Username { get; set; }

        public string TrackingValue { get; set; }

        public string TrackingValueOld { get; set; }

        public DateTime? LogDate { get; set; }

        public int ActionID { get; set; }

        public int? ActionTypeID { get; set; }

        public string SPName { get; set; }

        public byte IsInternal { get; set; }

        public int? CustID { get; set; }

        public string CustName { get; set; }

        public string ActionName { get; set; }
    }
}