// --------------------------------------------------------------------------
// <copyright file="SSOController.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
//   <change who="Jason.Ly" date="2014.4.14">Move out of User area.</change>
// </history>

namespace Fanex.NAP.CMS.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.NAP.CMS.Core.Web;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// SSO controller.
    /// </summary>
    public class SSOController : BaseController
    {
        #region members

        private readonly int _timeExipration = 30;

        private readonly IUserService _userService;
        private readonly IMenuItemService _menuItemService;
        private readonly ICurrentSession _currentSession;

        #endregion members

        #region ctors

        public SSOController()
            : this(null, null, null)
        {
        }

        public SSOController(
            IUserService userService,
            IMenuItemService menuItemService,
            ICurrentSession currentSession)
        {
            this._userService = userService ?? new UserService();
            this._menuItemService = menuItemService ?? new MenuItemService();
            this._currentSession = currentSession ?? new CurrentSession();
        }

        #endregion ctors

        /// <summary>
        /// Index page.
        /// </summary>
        /// <param name="userId">The user ID.</param>
        /// <param name="onuserId">The online user ID.</param>
        /// <param name="time">The SSO time.</param>
        /// <param name="redirectUrl">Redirected URL after SSO.</param>
        /// <returns>Action result.</returns>
        [AllowAnonymous]
        [HttpGet]
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult Index(string userId, string onuserId, string time, Uri redirectUrl = null)
        {
            string currentTime = this._userService.UserSSOGetTime();
            string forwardLink = redirectUrl == null ? FormsAuthentication.DefaultUrl : redirectUrl.ToString();

            userId = Cryptography.CfsDeCrypt(userId.Replace(" ", "+"));
            onuserId = Cryptography.CfsDeCrypt(onuserId.Replace(" ", "+"));
            time = Cryptography.CfsDeCrypt(time.Replace(" ", "+"));
            TimeSpan expiration = Convert.ToDateTime(currentTime) - Convert.ToDateTime(time);

            if (expiration.TotalSeconds <= this._timeExipration)
            {
                // do SSO check-in
                var criteria = new UserSSOCheckInUserCriteria
                {
                    UserId = Convert.ToInt32(userId),
                    SiteId = SiteId,
                    CheckOnUserId = Convert.ToInt32(onuserId),
                    OnUserId = 0,
                    IP = CurrentContext.IP,
                    NetSessionId = Session.SessionID,
                    LoginDate = DateTime.Now,
                    BrowserName = CurrentContext.BrowserName,
                    CountryCode = CurrentContext.CountryCode,
                    Domain = CurrentContext.DomainName,
                    Platform = CurrentContext.Platform
                };

                this._userService.UserSSOCheckIn(ref criteria);

                if (criteria.OnUserId != 0)
                {
                    var user = this._userService.GetById(criteria.UserId) as User;

                    AssignToSession(user, criteria.OnUserId);
                    Fanex.Nap.CMS.Infrastructure.Web.AppSession.CurrentUser = user;

                    return this.Redirect(forwardLink);
                }
            }

            return this.Redirect(FormsAuthentication.LoginUrl);
        }

        /// <summary>
        /// Login from Accounting site.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="onlineUserId">The online user identifier.</param>
        /// <param name="redirectUrl">The redirect URL.</param>
        /// <param name="newTab">Open in new tab and show menus or not.</param>
        /// <returns>The login result page.</returns>
        [HttpGet]
        [AllowAnonymous]
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult AccountingLogin(string username, string onlineUserId, Uri redirectUrl, bool newTab = false)
        {
            Session.Clear();
            username = Cryptography.CfsDeCrypt(username.Replace(" ", "+"));
            onlineUserId = Cryptography.CfsDeCrypt(onlineUserId.Replace(" ", "+"));

            var user = this._userService.GetByUsername(username) as User;

            if (user != null)
            {
                var criteria = new UserCheckInUserCriteria
                {
                    UserId = user.Id,
                    SiteId = this.SiteId,
                    NetSessionId = Session.SessionID,
                    Ip = CurrentContext.IP,
                    LoginDate = DateTime.Now,
                    BrowserName = CurrentContext.BrowserName,
                    CountryCode = CurrentContext.CountryCode,
                    Domain = CurrentContext.DomainName,
                    Platform = CurrentContext.Platform
                };

                // check-in user
                this._userService.UserCheckIn(ref criteria);

                AssignToSession(user, criteria.OnUserId);
                SessionManager.Set<bool>("ShowMenus", newTab);

                return this.Redirect(redirectUrl.ToString());
            }
            else
            {
                return this.Redirect(FormsAuthentication.LoginUrl);
            }
        }

        private void AssignToSession(User user, int onuserId)
        {
            _currentSession.UserId = user.Id;
            _currentSession.Username = user.Username;
            _currentSession.OnUserId = onuserId;
            _currentSession.SessionId = Session.SessionID;
            _currentSession.Level = _userService.GetUserSiteLevel(user.Id, SiteId);

            if (SiteId != 0)
            {
                _currentSession.MenuItemList = _menuItemService.GetByUserAndSiteIncludeGroup(
                    user.Id,
                    SiteId,
                    Environment,
                    true) as IList<MenuItem>;
            }

            AuthInfoUtils.CreateAuthInfo(user, _currentSession.OnUserId, CurrentContext, Response);
        }
    }
}