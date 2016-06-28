// --------------------------------------------------------------------------
// <copyright file="SmartPunterDetailLogInfo.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.23">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.ManageSmartPunters
{
    /// <summary>
    /// Smart punter detail log model.
    /// </summary>
    public class SmartPunterDetailLogInfo
    {
        public int? CustID { get; set; }

        public string Remark { get; set; }

        public string IP { get; set; }

        public int? ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}