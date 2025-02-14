// -----------------------------------------------------------------------
// <copyright file="UpdateMailingCriteria.cs">
// Copyright (c). All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Nap.CMS.NotificationSetting.Criteria.TurnoverControlNotification
{
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.NotificationSetting.Model.TurnoverControlNotification;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UpdateMailingCriteria : CommandBaseCriteria
    {
        [SpParam(Ignored = true)]
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
                return AppSettings.SMATurnoverMailFunctionName;
            }
        }

        [SpParam(Ignored = true)]
        public MailInfo MailInfo { get; set; }

        public string ToList => MailInfo?.ToList ?? string.Empty;

        public string CCList => MailInfo?.CCList ?? string.Empty;

        public string BCCList => MailInfo?.BCCList ?? string.Empty;

        public int SiteId => MailInfo?.SiteId ?? 0;

        public int UserId => AppSession.CurrentUser.Id;

        public override string GetSettingKey()
        {
            return this.IsUpdate ? "Acc_MailingListUpd" : "Acc_MailingListIns";
        }
    }
}