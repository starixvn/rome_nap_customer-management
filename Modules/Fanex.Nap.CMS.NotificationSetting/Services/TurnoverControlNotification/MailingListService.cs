// -----------------------------------------------------------------------
// <copyright file="MailingListService.cs">
// Copyright (c). All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Nap.CMS.NotificationSetting.Services.TurnoverControlNotification
{
    using System.Dynamic;
    using System.Linq;
    using Fanex.Nap.CMS.Infrastructure.Constants;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.NotificationSetting.Criteria.TurnoverControlNotification;
    using Fanex.Nap.CMS.NotificationSetting.Model.TurnoverControlNotification;
    using Fanex.Nap.CMS.NotificationSetting.Resources;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailingListService : ServiceBase, IMailingListService
    {
        /// <summary>
        /// Gets the email list.
        /// </summary>
        /// <returns>The email list.</returns>
        public MailingList MailingList()
        {
            var mailingList = new MailingList();
            var mailList = DynamicRepository.Fetch<MailInfo>(new GetMailingCriteria()).ToList();
            mailingList.MailInfo = mailList;
            mailingList.NonMailingList = string.Join(", ", mailList.Where(mail => !mail.IsMailExist).Select(mail => mail.SiteName));

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

            Result result = DoAction(criteria);

            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.UpdateSuccessfull;
            }

            return result;
        }

        public int UserGroupId()
        {
            GetUserGroupCriteria criteria = new GetUserGroupCriteria
            {
                UserId = AppSession.CurrentUser.Id
            };

            return DynamicRepository.Get<int>(criteria);
        }
    }
}