// --------------------------------------------------------------------------
// <copyright file="RobotAccountItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.30">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ManageRobots
{
    using System;
    using Fanex.NAP.CMS.Models.ManageRobots;
    using Fanex.NAP.Core.Web;

    /// <summary>
    /// Robot status item.
    /// </summary>
    [Serializable]
    public class RobotAccountItem
    {
        public RobotAccountItem(RobotAccountInfo rawData)
        {
            if (rawData != null)
            {
                this.CustID = rawData.CustID;
                this.Username = rawData.Username;
                this.Super = rawData.Super;
                this.Master = rawData.Master;
                this.Agent = rawData.Agent;
                this.IsActive = rawData.IsActive;
                this.Currency = rawData.Currency;
                this.Site = rawData.Site;
                this.AddedBy = rawData.Creator;
                this.AddedDate = rawData.CreateDate.ToShortFormatedDate();
                this.ModifiedBy = rawData.ModifyUser;
                this.ModifiedDate = rawData.LastModifyTime.HasValue ? rawData.LastModifyTime.ToShortFormatedDate() : string.Empty;
                this.Remark = rawData.Remark ?? string.Empty;
            }
        }

        public int CustID { get; set; }

        public string Username { get; set; }

        public string Super { get; set; }

        public string Master { get; set; }

        public string Agent { get; set; }

        public bool IsActive { get; set; }

        public string Currency { get; set; }

        public string Site { get; set; }

        public string AddedBy { get; set; }

        public string AddedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string ModifiedDate { get; set; }

        public string Remark { get; set; }
    }
}