// --------------------------------------------------------------------------
// <copyright file="SmartPunterLogItem.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.24">Created</change>
// </history>

namespace Fanex.NAP.CMS.ViewModels.ManageSmartPunters
{
    using System;
    using Fanex.Common;
    using Fanex.NAP.CMS.Models.ManageSmartPunters;
    using Fanex.NAP.Core.Web;

    /// <summary>
    /// Smart punter's logs view model.
    /// </summary>
    [Serializable]
    public class SmartPunterLogItem
    {
        public SmartPunterLogItem()
        {
        }

        public SmartPunterLogItem(SmartPunterLogInfo rawLog, SmartPunterDetailLogInfo rawDetail, SmartPunterDetailLogInfo oldRawDetail)
        {
            if (rawLog != null && rawDetail != null)
            {
                this.LogID = rawLog.LogID;
                this.LogDate = Converter.ToDateTime(rawLog.LogDate).ToFullFormatedDate();
                this.UserID = Converter.ToInt(rawLog.UserID);
                this.EditedBy = rawLog.Username;
                this.Action = rawLog.ActionName;
                this.SPName = rawLog.SPName;

                this.Detail = new SmartPunterDetailLogItem
                {
                    CustID = Converter.ToInt(rawLog.CustID), ////Converter.ToInt(rawDetail.CustID),
                    NewRemark = rawDetail.Remark,
                    ErrCode = rawDetail.ErrCode,
                    ErrMsg = rawDetail.ErrMsg,
                    IP = rawDetail.IP
                };

                if (oldRawDetail != null && !string.IsNullOrEmpty(oldRawDetail.Remark))
                {
                    this.Detail.OldRemark = oldRawDetail.Remark;
                }
            }
        }

        public long LogID { get; set; }

        public string LogDate { get; set; }

        public int UserID { get; set; }

        public string EditedBy { get; set; }

        public string Action { get; set; }

        public string SPName { get; set; }

        public SmartPunterDetailLogItem Detail { get; set; }
    }
}