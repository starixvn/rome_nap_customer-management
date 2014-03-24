// --------------------------------------------------------------------------
// <copyright file="UserSiteController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
// </history>

namespace Fanex.NAP.CMS.Areas.UserLogin.Controllers
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Common;
    using Fanex.NAP.CMS.Controllers;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// User site controller.
    /// </summary>
    public class UserSiteController : BaseController
    {
        #region members

        private ISiteService _siteService;
        private IUserService _userService;
        private ICurrentSession _currentSession;

        #endregion members

        #region ctors

        public UserSiteController()
            : this(null, null, null)
        {
        }

        public UserSiteController(ISiteService siteService, ICurrentSession currentSession, IUserService userService)
        {
            this._siteService = siteService ?? new SiteService();
            this._currentSession = currentSession ?? new CurrentSession();
            this._userService = userService ?? new UserService();
        }

        #endregion ctors

        #region actions

        /// <summary>
        /// View user sites.
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult Site()
        {
            if (HttpContext.User.Identity.IsAuthenticated && this._currentSession.UserId > 0)
            {
                var siteUrlList = this._siteService.GetSiteUrlByUser(this._currentSession.UserId, this.Environment);

                // for re-checkin
                ViewBag.OnUserId = this._currentSession.OnUserId;

                return View(siteUrlList);
            }

            return this.RedirectToAction("logout", "login");
        }

        /// <summary>
        /// Forward user to a site to SSO.
        /// </summary>
        /// <param name="id">The site URL.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult Forward(string id)
        {
            int siteUrlId = Convert.ToInt32(id);
            var siteUrl = this._siteService.GetSiteUrlById(siteUrlId);

            string time = this._userService.UserSSOGetTime();

            return new RedirectResult(
                string.Format(
                    "{0}/userlogin/sso?userid={1}&onuserid={2}&time={3}",
                    siteUrl.Url.Trim(),
                    Cryptography.CfsEnCrypt(this._currentSession.UserId.ToString()),
                    Cryptography.CfsEnCrypt(this._currentSession.OnUserId.ToString()),
                    Cryptography.CfsEnCrypt(time)));
        }

        #endregion actions
    }
}