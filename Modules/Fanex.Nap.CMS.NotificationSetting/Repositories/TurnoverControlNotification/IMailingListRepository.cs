// -----------------------------------------------------------------------
// <copyright file="IMailingListRepository.cs">
// Copyright (c). All rights reserved.
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