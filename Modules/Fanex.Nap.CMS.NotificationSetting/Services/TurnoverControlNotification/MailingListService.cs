// -----------------------------------------------------------------------
// <copyright file="MailingListService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Nap.CMS.NotificationSetting.Services.TurnoverControlNotification
{
    using Fanex.Nap.CMS.Infrastructure.Constants;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.NotificationSetting.Criteria.TurnoverControlNotification;
    using Fanex.Nap.CMS.NotificationSetting.Model.TurnoverControlNotification;
    using Fanex.Nap.CMS.NotificationSetting.Repositories.TurnoverControlNotification;
    using Fanex.Nap.CMS.NotificationSetting.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailingListService : IMailingListService
    {
        private readonly IMailingListRepository _repository;

        public MailingListService()
            : this(null)
        {
        }

        public MailingListService(IMailingListRepository repository)
        {
            _repository = repository ?? new MailingListRepository();
        }

        /// <summary>
        /// Gets the email list.
        /// </summary>
        /// <returns>The email list.</returns>
        public MailingList MailingList()
        {
            var mailingList = _repository.GetMailingList(new GetMailingCriteria());
            return mailingList;
        }

        /// <summary>
        /// Updates the email list.
        /// </summary>
        /// <param name="mailInfo">The email information.</param>
        /// <param name="isUpdate">If set to <c>true</c> [is update].</param>
        /// <returns>The result of update action.</returns>
        public Result UpdateMailingList(MailInfo mailInfo, bool isUpdate)
        {
            var result = new Result();
            var criteria = new UpdateMailingCriteria
            {
                IsUpdate = isUpdate,
                MailInfo = mailInfo
            };

            criteria.MailInfo.ToList = string.IsNullOrEmpty(criteria.MailInfo.ToList) ?
                string.Empty :
                criteria.MailInfo.ToList;
            criteria.MailInfo.CCList = string.IsNullOrEmpty(criteria.MailInfo.CCList) ?
                string.Empty :
                criteria.MailInfo.CCList;
            criteria.MailInfo.BCCList = string.IsNullOrEmpty(criteria.MailInfo.BCCList) ?
                string.Empty :
                criteria.MailInfo.BCCList;

            result = _repository.DoAction(criteria);

            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.UpdateSuccessfull;
            }

            return result;
        }

        public int UserGroupId()
        {
            GetUserGroupCriteria criteria = new GetUserGroupCriteria();
            criteria.UserId = AppSession.CurrentUser.Id;
            int userGroupdId = _repository.GetUserGroup(criteria);
            return userGroupdId;
        }
    }
}