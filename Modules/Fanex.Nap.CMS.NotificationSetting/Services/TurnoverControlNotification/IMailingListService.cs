// -----------------------------------------------------------------------
// <copyright file="IMailingListService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Nap.CMS.NotificationSetting.Services.TurnoverControlNotification
{
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.NotificationSetting.Model.TurnoverControlNotification;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMailingListService
    {
        MailingList MailingList();

        Result UpdateMailingList(MailInfo mailInfo, bool isUpdate);

        int UserGroupId();
    }
}