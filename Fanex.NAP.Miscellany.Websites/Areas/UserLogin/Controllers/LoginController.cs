// --------------------------------------------------------------------------
// <copyright file="LoginController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
// </history>

namespace Fanex.NAP.Miscellany.Areas.UserLogin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.NAP.Core.Criteria.User;
    using Fanex.NAP.Core.Model;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Session;
    using Fanex.NAP.Miscellany.Controllers;

    /// <summary>
    /// Login controller.
    /// </summary>
    public class LoginController : BaseController
    {
        #region members

        private IUserService _userService;
        private IGroupService _groupService;
        private ISiteService _siteService;
        private IMenuItemService _menuItemService;
        private ICurrentSession _currentSession;
        private IMaintenanceService _maintenanceService;

        #endregion members

        #region ctors

        public LoginController()
            : this(null, null, null, null, null, null)
        {
        }

        public LoginController(
            IUserService userService,
            IGroupService groupService,
            ISiteService siteService,
            IMenuItemService menuItemService,
            ICurrentSession currentSession,
            IMaintenanceService maintenanceService)
        {
            this._userService = userService ?? new UserService();
            this._groupService = groupService ?? new GroupService();
            this._siteService = siteService ?? new SiteService();
            this._menuItemService = menuItemService ?? new MenuItemService();
            this._currentSession = currentSession ?? new CurrentSession();
            this._maintenanceService = maintenanceService ?? new MaintenanceService();
        }

        #endregion ctors

        #region sign in

        /// <summary>
        /// Index page.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var maintenanceInfo = this._maintenanceService.GetDowntimeInfo();

            if (maintenanceInfo.IsDowntime)
            {
                return this.RedirectToAction("Offline", "Common");
            }

            return View();
        }

        /// <summary>
        /// Index page.
        /// </summary>
        /// <param name="username">The login username.</param>
        /// <param name="password">The login password.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [ValidateInput(false)]
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public ActionResult Index(string username, string password)
        {
            if (!username.IsNullOrTrimEmpty() && !password.IsNullOrTrimEmpty())
            {
                var user = this._userService.GetByUsername(username.Trim()) as User;

                if (user != null)
                {
                    password = Cryptography.CfsEnCode(password);

                    if (password == user.Password)
                    {
                        var group = user.Group == null ? null : this._groupService.GetById(user.Group.Id);
                        var siteLevelList = this._siteService.GetSiteLevelByUser(user.Id, this.Environment); // gets all sites of logging-in user

                        if ((group == null || group.IsActive) && siteLevelList.Where(x => x.IsActive).Any())
                        {
                            // check-in user
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

                            this._userService.UserCheckIn(ref criteria);

                            // saves user session
                            this._currentSession.Level = this._userService.GetUserSiteLevel(user.Id, this.SiteId);
                            this._currentSession.UserId = user.Id;
                            this._currentSession.Username = username;
                            this._currentSession.OnUserId = criteria.OnUserId;
                            this._currentSession.SessionId = Session.SessionID.ToString();

                            // current site is not NAP Hub
                            if (this.SiteId != 0)
                            {
                                this.CreateUserMenu(user);
                            }

                            this.CreateAuthInfo(user);

                            // if the user login directly, not through NAP Hub
                            if (this.IsSiteLogin == 1)
                            {
                                // checks if logging-in user is active on this site
                                bool userIsActiveOnThisSite = false;

                                foreach (var siteLevel in siteLevelList)
                                {
                                    if (siteLevel.Site.Id == this.SiteId && siteLevel.IsActive)
                                    {
                                        userIsActiveOnThisSite = true;
                                    }
                                }

                                if (userIsActiveOnThisSite)
                                {
                                    return this.Redirect(FormsAuthentication.DefaultUrl);
                                }
                                else
                                {
                                    this.DoSignOut();
                                    ViewBag.Message = ViewResources.Your_account_was_suspended;
                                }
                            }
                            else
                            {
                                // user has access to multiple sites
                                if (siteLevelList.Where(s => s.IsActive).Count() > 1)
                                {
                                    return this.RedirectToAction("Site", "UserSite");
                                }

                                // user has acces to one site
                                var site = siteLevelList.Where(s => s.IsActive).First().Site;

                                var listSiteUrl = this._siteService.GetSiteUrlBySiteId(site.Id, this.Environment).Where(u => u.IsActive).ToList();

                                if (listSiteUrl.Count() > 1)
                                {
                                    return this.RedirectToAction("Site", "UserSite");
                                }

                                return this.RedirectToAction("Forward", "UserSite", new { id = listSiteUrl.First().SiteUrlId });
                            }
                        }
                        else
                        {
                            // user account was suspended on all sites
                            if (siteLevelList.Count() > 0)
                            {
                                ViewBag.Message = ViewResources.Your_account_was_suspended;
                            }
                            else
                            {
                                // logging-in user belongs to no site
                                ViewBag.Message = ViewResources.You_have_no_authorization;
                            }
                        }
                    }
                    else
                    {
                        // wrong password
                        ViewBag.Message = ViewResources.Invalid_Username_or_Password;
                    }
                }
                else
                {
                    // user doesn't exist
                    ViewBag.Message = ViewResources.Invalid_Username_or_Password;
                }
            }
            else
            {
                ViewBag.Message = "Invalid input!";
            }

            return View();
        }

        #endregion sign in

        /// <summary>
        /// User log out.
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult LogOut()
        {
            this.DoSignOut();

            return this.Redirect(FormsAuthentication.LoginUrl);
        }

        /// <summary>
        /// User re-check login.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult ReCheckLogin()
        {
            var maintenanceInfo = this._maintenanceService.GetDowntimeInfo();

            if (maintenanceInfo.IsDowntime)
            {
                return this.Json("-1");
            }

            // check if not nap hub
            if (this.SiteId != 0)
            {
                var siteStatus = this._siteService.GetStatusById(this.SiteId);

                if (!siteStatus)
                {
                    return this.Json("1");
                }
            }

            UserReCheckInUserCriteria criteria = new UserReCheckInUserCriteria();
            criteria.OnUserId = Convert.ToInt32(this._currentSession.OnUserId);
            this._userService.ReUserCheckIn(ref criteria);

            return this.Json(criteria.IsKickedOut ? "1" : "0");
        }

        #region helpers

        /// <summary>
        /// Create user menu.
        /// </summary>
        /// <param name="user">The login user.</param>
        private void CreateUserMenu(User user)
        {
            this._currentSession.MenuItemList = (IList<MenuItem>)this._menuItemService.GetByUserAndSiteIncludeGroup(
                                                                                    user.Id,
                                                                                    SiteId,
                                                                                    this.Environment,
                                                                                    true);
        }

        /// <summary>
        /// Create authorize information.
        /// </summary>
        /// <param name="user">The login user.</param>
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

        #endregion helpers

        /// <summary>
        /// Does the sign out.
        /// </summary>
        private void DoSignOut()
        {
            this._userService.UserLogOut(new LogOutUserCriteria
            {
                OnUserId = this._currentSession.OnUserId
            });
            FormsAuthentication.SignOut();
            Response.Cookies.Clear();
            this._currentSession.ClearAll();
        }
    }
}