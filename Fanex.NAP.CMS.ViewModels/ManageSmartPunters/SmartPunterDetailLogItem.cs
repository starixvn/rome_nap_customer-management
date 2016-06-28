// --------------------------------------------------------------------------
// <copyright file="SmartPunterDetailLogItem.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.23">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ManageSmartPunters
{
    using System;

    /// <summary>
    /// Smart punter's detail log view model.
    /// </summary>
    [Serializable]
    public class SmartPunterDetailLogItem
    {
        public int CustID { get; set; }

        public string OldRemark { get; set; }

        public string NewRemark { get; set; }

        public string IP { get; set; }

        public int? ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}