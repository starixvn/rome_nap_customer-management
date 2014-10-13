// -----------------------------------------------------------------------
// <copyright file="UpdateMailingCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Nap.CMS.NotificationSetting.Criteria.TurnoverControlNotification
{
    using System.Collections;
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.NotificationSetting.Model.TurnoverControlNotification;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateMailingCriteria : CriteriaBase
    {
        public bool IsUpdate { get; set; }

        public int FuncId
        {
            get
            {
                return AppSettings.SMATurnoverMailFunctionId;
            }
        }

        public string FuncName
        {
            get
            {
                return "[SMATurnoverMailingList]";
            }
        }

        public MailInfo MailInfo { get; set; }

        public override string GetSettingKey()
        {
            return this.IsUpdate ? "Acc_MailingListUpd" : "Acc_MailingListIns";
        }

        /// <summary>
        /// Gets the stored procedure parameters.
        /// </summary>
        /// <returns>The parameters.</returns>
        public override Hashtable GetSpParams()
        {
            Hashtable table = new Hashtable();

            table.Add("Userid", AppSession.CurrentUser.Id);
            table.Add("Siteid", this.MailInfo.SiteId);
            table.Add("FuncID", this.FuncId);
            table.Add("FuncName", this.FuncName);
            table.Add("tolist", this.MailInfo.ToList);
            table.Add("cclist", this.MailInfo.CCList);
            table.Add("bcclist", this.MailInfo.BCCList);
            table.Add("errcode", 0);
            table.Add("errmsg", string.Empty);

            return table;
        }
    }
}