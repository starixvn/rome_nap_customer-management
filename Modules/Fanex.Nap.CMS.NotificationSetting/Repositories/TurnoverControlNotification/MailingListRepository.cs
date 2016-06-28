// -----------------------------------------------------------------------
// <copyright file="MailingListRepository.cs">
// Copyright (c). All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Nap.CMS.NotificationSetting.Repositories.TurnoverControlNotification
{
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.Nap.CMS.NotificationSetting.Criteria.TurnoverControlNotification;
    using Fanex.Nap.CMS.NotificationSetting.Model.TurnoverControlNotification;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailingListRepository : BaseRepository, IMailingListRepository
    {
        /// <summary>
        /// Gets the email list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The email list.</returns>
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed. Suppression is OK here.")]
        public MailingList GetMailingList(GetMailingCriteria criteria)
        {
            var mailingList = new MailingList();
            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var _params = criteria.GetSpParams();
                IDataReader reader = objectDb.ExecuteReader(_params);
                int index = 0;
                while (reader.Read())
                {
                    MailInfo mailInfo = new MailInfo
                    {
                        SiteId = Converter.ToInt(reader["siteid"]),
                        SiteName = Converter.ToString2(reader["site"]),
                        ToList = Converter.ToString2(reader["tolist"]),
                        CCList = Converter.ToString2(reader["cclist"]),
                        BCCList = Converter.ToString2(reader["bcclist"]),
                        IsMailExist = Converter.ToInt(reader["isavailabe"]) == 1 ? true : false
                    };

                    mailingList.MailInfo.Add(mailInfo);

                    if (!mailInfo.IsMailExist)
                    {
                        mailingList.NonMailingList += index == 0 ?
                            mailInfo.SiteName :
                            string.Format(", {0}", mailInfo.SiteName);
                        index++;
                    }
                }

                reader.Close();
            }

            return mailingList;
        }

        /// <summary>
        /// Gets the user group.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The user group.</returns>
        public int GetUserGroup(GetUserGroupCriteria criteria)
        {
            int userGroup = 0;

            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                var _params = criteria.GetSpParams();
                IDataReader reader = objectDb.ExecuteReader(_params);

                while (reader.Read())
                {
                    userGroup = Converter.ToInt(reader["GroupId"]);
                }

                reader.Close();
            }

            return userGroup;
        }
    }
}