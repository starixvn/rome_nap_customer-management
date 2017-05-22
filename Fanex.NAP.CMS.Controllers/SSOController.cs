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
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.NAP.CMS.Core.Web;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Auth;
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

                    // saves user session & auth info
                    this._currentSession.UserId = user.Id;
                    this._currentSession.Username = user.Username;
                    this._currentSession.OnUserId = criteria.OnUserId;
                    this._currentSession.SessionId = Session.SessionID;
                    this._currentSession.Level = this._userService.GetUserSiteLevel(user.Id, this.SiteId);

                    if (this.SiteId != 0)
                    {
                        this._currentSession.MenuItemList = this._menuItemService.GetByUserAndSiteIncludeGroup(
                                                                                user.Id,
                                                                                SiteId,
                                                                                this.Environment,
                                                                                true) as IList<MenuItem>;
                    }

                    this.CreateAuthInfo(user);
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

                // saves user session & auth info
                this._currentSession.UserId = user.Id;
                this._currentSession.Username = user.Username;
                this._currentSession.OnUserId = criteria.OnUserId; ////Convert.ToInt32(onlineUserId);
                this._currentSession.SessionId = Session.SessionID;
                this._currentSession.Level = this._userService.GetUserSiteLevel(user.Id, this.SiteId);

                if (this.SiteId != 0)
                {
                    this._currentSession.MenuItemList = this._menuItemService.GetByUserAndSiteIncludeGroup(
                                                                                  user.Id,
                                                                                  SiteId,
                                                                                  this.Environment,
                                                                                  true) as IList<MenuItem>;
                }

                this.CreateAuthInfo(user);
                SessionManager.Set<bool>("ShowMenus", newTab);

                return this.Redirect(redirectUrl.ToString());
            }
            else
            {
                return this.Redirect(FormsAuthentication.LoginUrl);
            }
        }

        #region utilities

        /// <summary>
        /// Create authorize information.
        /// </summary>
        /// <param name="user">The user object.</param>
        private void CreateAuthInfo(User user)
        {
            var currentDomain = CurrentContext.GetCurrentDomain();

            var authInfo = new AuthInfo();

            authInfo.UserId = user.Id;
            authInfo.OnUserId = this._currentSession.OnUserId;

            var authTicket = new FormsAuthenticationTicket(
                 1,
                user.Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(20000),
                false,
                authInfo.ToXml(),
                currentDomain.Path);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Path = currentDomain.Path,
                Name = FormsAuthentication.FormsCookieName
            };

            Response.Cookies.Add(authCookie);
        }

        #endregion utilities
    }
}