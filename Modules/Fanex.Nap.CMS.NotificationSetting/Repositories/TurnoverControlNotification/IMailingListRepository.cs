// -----------------------------------------------------------------------
// <copyright file="IMailingListRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Nap.CMS.NotificationSetting.Repositories.TurnoverControlNotification
{
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.Nap.CMS.NotificationSetting.Criteria.TurnoverControlNotification;
    using Fanex.Nap.CMS.NotificationSetting.Model.TurnoverControlNotification;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMailingListRepository : IBaseRepository
    {
        MailingList GetMailingList(GetMailingCriteria criteria);

        int GetUserGroup(GetUserGroupCriteria criteria);
    }
}