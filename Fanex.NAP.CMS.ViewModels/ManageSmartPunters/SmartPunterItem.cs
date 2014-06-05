// --------------------------------------------------------------------------
// <copyright file="SmartPunterItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.5">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ManageSmartPunters
{
    using System;
    using Fanex.NAP.CMS.Core.Resources;
    using Fanex.NAP.CMS.Models.ManageSmartPunters;
    using Fanex.NAP.Core.Web;

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
                this.Nickname = rawData.Nickname;
                this.Super = rawData.Super;
                this.Master = rawData.Master;
                this.Agent = rawData.Agent;
                this.Status = ConvertStatus(rawData.StatusID);
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

        public string Currency { get; set; }

        public string Site { get; set; }

        public string AddedBy { get; set; }

        public string AddedDate { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// Converts the status ID to status name.
        /// </summary>
        /// <param name="statusID">The status identifier.</param>
        /// <returns>The status name.</returns>
        private string ConvertStatus(byte? statusID)
        {
            var status = string.Empty;

            if (statusID.HasValue)
            {
                switch (statusID)
                {
                    case 0:
                        status = ViewLang.Open;
                        break;

                    case 1:
                        status = ViewLang.Suspended;
                        break;

                    case 2:
                        status = ViewLang.Closed;
                        break;
                }
            }

            return status;
        }
    }
}