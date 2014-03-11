// --------------------------------------------------------------------------
// <copyright file="RequestItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.6">Created</change>
// </history>

namespace Fanex.NAP.Miscellany.ViewModels.ConfigureCustomerAccess
{
    using System.Collections.Generic;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Miscellany.Models.ConfigureCustomerAccess;

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
                this.Creator = rawData.CreatedBy;
                this.LastModifiedBy = rawData.UpdatedBy;

                if (!string.IsNullOrEmpty(rawData.CountryCode) && countryList.ContainsKey(rawData.CountryCode))
                {
                    this.Country = countryList[rawData.CountryCode];
                }

                if (rawData.ExpiryDate.HasValue)
                {
                    this.ExpiredDate = rawData.ExpiryDate.Value.ToShortFormatedDate();
                }

                if (rawData.CreatedDate.HasValue)
                {
                    this.CreatedDate = rawData.CreatedDate.Value.ToShortFormatedDate();
                }

                if (rawData.UpdatedDate.HasValue)
                {
                    this.LastModifiedDate = rawData.UpdatedDate.Value.ToShortFormatedDate();
                }
            }
        }

        public int RequestId { get; set; }

        public int CustId { get; set; }

        public string Username { get; set; }

        public string ExpiredDate { get; set; }

        public string CountryCode { get; set; }

        public string Country { get; set; }

        public string IP { get; set; }

        public string Remark { get; set; }

        public string Creator { get; set; }

        public string CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public string LastModifiedDate { get; set; }
    }
}