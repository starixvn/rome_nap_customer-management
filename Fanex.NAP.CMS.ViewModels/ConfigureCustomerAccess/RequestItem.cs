// --------------------------------------------------------------------------
// <copyright file="RequestItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ConfigureCustomerAccess
{
    using System;
    using System.Collections.Generic;
    using Fanex.NAP.CMS.Core.Helpers;
    using Fanex.NAP.CMS.Models.ConfigureCustomerAccess;

    /// <summary>
    /// View model for new/updated request.
    /// </summary>
    public class RequestItem
    {
        public RequestItem()
        {
        }

        public RequestItem(RequestInfo rawData, Dictionary<string, string> countryList)
        {
            if (rawData != null)
            {
                this.RequestId = rawData.RequestID;
                this.CustId = rawData.CustID;
                this.Username = rawData.Username;
                this.CountryCode = rawData.CountryCode;
                this.IP = rawData.IP;
                this.Remark = rawData.Remark;
                this.CreatedBy = rawData.CreatedBy;
                this.LastModifiedBy = rawData.UpdatedBy;
                this.ExpiredDate = rawData.ExpiryDate.ToShortFormatedDate();
                this.CreatedDate = rawData.CreatedDate.ToShortFormatedDate();
                this.LastModifiedDate = rawData.UpdatedDate.ToShortFormatedDate();

                if (!string.IsNullOrEmpty(rawData.CountryCode) && countryList.ContainsKey(rawData.CountryCode))
                {
                    this.Country = countryList[rawData.CountryCode];
                }

                if (rawData.ExpiryDate.HasValue)
                {
                    this.IsExpired = rawData.ExpiryDate.Value < DateTime.Now;
                }
            }
        }

        public int RequestId { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public string ExpiredDate { get; set; }

        public bool IsExpired { get; set; }

        public string CountryCode { get; set; }

        public string Country { get; set; }

        public string IP { get; set; }

        public string Remark { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public string LastModifiedDate { get; set; }
    }
}