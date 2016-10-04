// --------------------------------------------------------------------------
// <copyright file="SmartPunterItem.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.5">Created</change>
// </history>

using Fanex.NAP.CMS.Core.Web;

namespace Fanex.NAP.CMS.ViewModels.ManageSmartPunters
{
    using System;
    using Fanex.NAP.CMS.Core.Helpers;
    using Fanex.NAP.CMS.Core.Resources;
    using Fanex.NAP.CMS.Models.ManageSmartPunters;

    /// <summary>
    /// Smart punter item.
    /// </summary>
    [Serializable]
    public class SmartPunterItem
    {
        public SmartPunterItem(SmartPunterInfo rawData)
        {
            if (rawData != null)
            {
                this.CustID = rawData.CustID;
                this.Username = rawData.Username;
                this.Nickname = rawData.Username2;
                this.Super = rawData.Super;
                this.Master = rawData.Master;
                this.Agent = rawData.Agent;
                this.Status = ConvertStatus(rawData.Reject, rawData.Closed);
                this.RoleId = rawData.RoleId;
                this.Currency = rawData.Currency;
                this.Site = rawData.Site;
                this.AddedBy = rawData.Creator;
                this.AddedDate = rawData.CreateDate.ToShortFormatedDate();
                this.Remark = rawData.Remark ?? string.Empty;
            }
        }

        public int CustID { get; set; }

        public string Username { get; set; }

        public string Nickname { get; set; }

        public string Super { get; set; }

        public string Master { get; set; }

        public string Agent { get; set; }

        public string Status { get; set; }

        public int RoleId { get; set; }

        public string RoleName
        {
            get
            {
                return CacheRef.GetLevel((byte?)RoleId);
            }
        }

        public string Currency { get; set; }

        public string Site { get; set; }

        public string AddedBy { get; set; }

        public string AddedDate { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// Converts the status ID to status name.
        /// </summary>
        /// <param name="reject">The suspended status.</param>
        /// <param name="closed">The closed status.</param>
        /// <returns>The status name.</returns>
        private string ConvertStatus(bool reject, bool closed)
        {
            var status = string.Empty;

            if (closed)
            {
                status = ViewLang.Closed;
            }
            else if (reject)
            {
                status = ViewLang.Suspended;
            }
            else
            {
                status = ViewLang.Open;
            }

            return status;
        }
    }
}