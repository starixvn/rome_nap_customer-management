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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.NAP.Client;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Models;
    using Fanex.NAP.Client.Models.ParameterObject;
    using Fanex.NAP.Client.Services;

    /// <summary>
    /// SSO controller.
    /// </summary>
    public class SSOController : BaseController
    {
        #region members

        private readonly int _timeExipration = 30;

        private readonly IUserService _userService;
        private readonly INapSession _currentSession;
        private readonly ISecurityService _securityService;

        #endregion members

        #region ctors

        public SSOController()
            : this(null, null)
        {
        }

        public SSOController(
            IUserService userService,
            ISecurityService securityService)
        {
            this._userService = userService ?? new UserService();
            this._currentSession = NapClientManager.NapSession;
            _securityService = securityService ?? new SecurityService();
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
            string currentTime = _securityService.GetTime();
            string forwardLink = redirectUrl == null ? FormsAuthentication.DefaultUrl : redirectUrl.ToString();

            userId = Cryptography.CfsDeCrypt(userId.Replace(" ", "+"));
            onuserId = Cryptography.CfsDeCrypt(onuserId.Replace(" ", "+"));
            time = Cryptography.CfsDeCrypt(time.Replace(" ", "+"));
            TimeSpan expiration = Convert.ToDateTime(currentTime) - Convert.ToDateTime(time);

            if (expiration.TotalSeconds <= this._timeExipration)
            {
                // do SSO check-in
                var loginParam = new SSOLoginParameter
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
                
                var onlineUserId = _securityService.SSOLogin(loginParam);

                if (onlineUserId != 0)
                {
                    var user = this._userService.Get(loginParam.UserId);

                    AssignToSession(user, onlineUserId);
                    Fanex.Nap.CMS.Infrastructure.Web.AppSession.CurrentUser = user;

                    return this.Redirect(forwardLink);
                }
            }

            return this.Redirect(FormsAuthentication.LoginUrl);
        }

        private void AssignToSession(User user, int onuserId)
        {
            _currentSession.UserId = user.Id;
            _currentSession.UserName = user.Username;
            _currentSession.OnUserId = onuserId;
            _currentSession.Level = _userService.GetUserLevel(user.Id, SiteId);

            if (SiteId != 0)
            {
                _currentSession.MenuItemList = _userService.GetMenuItem(
                    user.Id,
                    SiteId,
                    Environment).ToList();
            }

            AuthInfoUtils.CreateAuthInfo(_currentSession, HttpContext);
        }
    }
}