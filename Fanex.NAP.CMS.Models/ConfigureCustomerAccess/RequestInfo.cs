// --------------------------------------------------------------------------
// <copyright file="RequestInfo.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

namespace Fanex.NAP.CMS.Models.ConfigureCustomerAccess
{
    using System;

    /// <summary>
    /// View model for new/updated request.
    /// </summary>
    public class RequestInfo
    {
        public int RequestID { get; set; }

        public int CustID { get; set; }

        public string Username { get; set; }

        public byte RoleID { get; set; }

        public string CountryCode { get; set; }

        public string IP { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public string Remark { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}