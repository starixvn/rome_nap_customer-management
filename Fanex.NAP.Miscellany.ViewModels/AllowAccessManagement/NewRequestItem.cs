// --------------------------------------------------------------------------
// <copyright file="NewRequestItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.ViewModels.AllowAccessManagement
{
    using System;

    /// <summary>
    /// View model for new/updated request.
    /// </summary>
    public class NewRequestItem
    {
        public int RequestId { get; set; }

        public string Country { get; set; }

        public string IP { get; set; }

        public DateTime ExpiredDate { get; set; }

        public string Remark { get; set; }

        public bool IsNewRequest { get; set; }
    }
}