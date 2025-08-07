// --------------------------------------------------------------------------
// <copyright file="AppSession.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.Nap.CMS.Infrastructure.Web
{
    using System;
    using System.Web;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Constants;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.Client.Models;

    /// <summary>
    /// App Session.
    /// </summary>
    public static class AppSession
    {
        public const string BalanceUpSessionKey = "Fanex.Nap.CMS.Infrastructure.Web.BalanceUpSessionKey";
        public const string CurrentActionNameKey = "Fanex.Nap.CMS.Infrastructure.Web.AppSession.CurrentActionName";

        public static IDynamicRepository Repository
        {
            get
            {
                return new DynamicRepository();
            }
        }

        /// <summary>
        /// Gets or sets current login user.
        /// </summary>
        /// <value>
        /// The current user.
        /// </value>
        public static User CurrentUser
        {
            get
            {
                return SessionManager.GetSession(HttpContext.Current.Session, SessionKeyConstants.CurrentLogInUser) as User;
            }

            set
            {
                SessionManager.AddSession(HttpContext.Current.Session, SessionKeyConstants.CurrentLogInUser, value);
            }
        }

        public static BalanceUpDayInfo BalanceUpDate
        {
            get
            {
                BalanceUpDayInfo balanceUp = CacheManager.Get<BalanceUpDayInfo>(BalanceUpSessionKey);
                if (balanceUp == null)
                {
                    balanceUp = Repository.Get<BalanceUpDayInfo>(new GetBalanceUpDayCriteria());
                    CacheManager.Set<BalanceUpDayInfo>(BalanceUpSessionKey, balanceUp, new TimeSpan(0, 0, 5, 0));
                }

                return balanceUp;
            }
        }
    }
}
