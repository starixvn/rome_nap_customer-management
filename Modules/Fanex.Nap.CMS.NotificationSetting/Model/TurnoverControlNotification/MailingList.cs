// -----------------------------------------------------------------------
// <copyright file="MailingList.cs">
// Copyright (c). All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Nap.CMS.NotificationSetting.Model.TurnoverControlNotification
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailingList
    {
        public MailingList()
        {
            MailInfo = new List<MailInfo>();
        }

        public List<MailInfo> MailInfo { get; set; }

        public string NonMailingList { get; set; }
    }
}