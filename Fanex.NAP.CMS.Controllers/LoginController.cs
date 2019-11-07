// --------------------------------------------------------------------------
// <copyright file="LoginController.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
//   <change who="Jason.Ly" date="2014.4.14">Move out of User area.</change>
//   <change who="Jason.Ly" date="2014.7.29">Separate Login action into some private functions.</change>
// </history>

namespace Fanex.NAP.CMS.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common.Extensions;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Session;
    using NAP.Core.Models;
    using NAP.Core.Models.ParameterObject;
    using ONEbook.UM.Models;

    /// <summary>
    /// Login controller.
    /// </summary>
    public class LoginController : BaseController
    {
        #region Members

        private const string LoginFormToken = "_SecureLoginFormToken";

        private readonly IUserService _userService;
        private readonly IGroupService _groupService;
        private readonly ISiteService _siteService;
        private readonly ICurrentSession _currentSession;
        private readonly ONEbook.UM.Services.IMaintenanceService _maintenanceService;
        private readonly HashAlgorithm _hashAlgorithm;
        private readonly ISecurityService _securityService;

        #endregion Members

        #region Constructors

        public LoginController()
            : this(null, null, null, null, null, null)
        {
        }

        public LoginController(
            IUserService userService,
            IGroupService groupService,
            ISiteService siteService,
            ICurrentSession currentSession,
           ONEbook.UM.Services.IMaintenanceService maintenanceService,
           ISecurityService securityService)
        {
            this._userService = userService ?? new UserService();
            this._groupService = groupService ?? new GroupService();
            this._siteService = siteService ?? new SiteService();
            this._currentSession = currentSession ?? new CurrentSession();
            this._maintenanceService = maintenanceService ?? new ONEbook.UM.Services.Impl.MaintenanceService();
            _hashAlgorithm = new SHA256Managed();
            _securityService = securityService ?? new SecurityService();
        }

        #endregion Constructors

        #region Properties

        public string FormToken
        {
            get
            {
                if (Session[LoginFormToken] == null)
                {
                    return string.Empty;
                }

                return Session[LoginFormToken].ToString();
            }

            set
            {
                Session[LoginFormToken] = value;
            }
        }

        #endregion Properties

        #region Sign in

        /// <summary>
        /// Index page.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (!NapSettings.Settings.IsNapHub && !EnableDirectLogin)
            {
                // if login-directly feature is disabled then redirect users to NAP Hub
                return Redirect(NapSettings.Settings.HubUrl);
            }

            if (_maintenanceService.IsUnderMaintenance())
            {
                return RedirectToAction("Offline");
            }

            GenerateToken();

            return View();
        }

        /// <summary>
        /// Index page.
        /// </summary>
        /// <param name="username">The login username.</param>
        /// <param name="password">The login password.</param>
        /// <param name="loginToken">Login token.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string username, string password, string loginToken)
        {
            if (!username.IsNullOrTrimEmpty() && !password.IsNullOrTrimEmpty() && CheckValidToken(loginToken))
            {
                var user = this._userService.Get(username.Trim());

                if (user != null)
                {
                    string hashPassword = GetHash(user.Password + loginToken);

                    if (password == hashPassword)
                    {
                        ClearToken();

                        if (CheckUserSitePermission(user))
                        {
                            // Check in user.
                            this.CheckIn(user);
                            this.CreateUserMenu(user);
                            AuthInfoUtils.CreateAuthInfo(_currentSession, HttpContext);

                            Fanex.Nap.CMS.Infrastructure.Web.AppSession.CurrentUser = user;
                            return Redirect(FormsAuthentication.DefaultUrl);
                        }
                        else
                        {
                            // logging-in user belongs to no site
                            ViewBag.Message = ViewResources.You_have_no_authorization;
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

            GenerateToken();

            return View();
        }

        #endregion Sign in

        #region Other Actions

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
            if (_maintenanceService.IsUnderMaintenance())
            {
                return Json("-1");
            }

            // check if not nap hub
            if (SiteId != 0)
            {
                var siteStatus = _siteService.IsActivated(SiteId);

                if (!siteStatus)
                {
                    return Json("1");
                }
            }

            var isKickedOut = false;

            try
            {
                isKickedOut = NapSettings.Settings.EnableReCheckIn
                    && _securityService.ReCheckIn(_currentSession.OnUserId, SiteId) > 0;
            }
            catch (Exception)
            {
                isKickedOut = true;
            }

            return Json(isKickedOut ? "1" : "0");
        }

        /// <summary>
        /// Offline this instance.
        /// </summary>
        /// <returns>Return status of maintenance.</returns>
        public ActionResult Offline()
        {
            if (!_maintenanceService.IsUnderMaintenance())
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            UnderMaintenanceTime downTime = _maintenanceService.GetUnderMaintenanceTime();
            ViewBag.UmtFrom = downTime.From;
            ViewBag.UmtTo = downTime.To;

            return View();
        }

        #endregion Other Actions

        #region Helpers

        /// <summary>
        /// Create user menu.
        /// </summary>
        /// <param name="user">The login user.</param>
        private void CreateUserMenu(User user)
            => _currentSession.MenuItemList = _userService.GetMenuItem(user.Id, SiteId, Environment, true).ToList();

        /// <summary>
        /// Checks in an user.
        /// </summary>
        /// <param name="user">The user information.</param>
        private void CheckIn(User user)
        {
            // check-in user
            var loginParameter = new LoginParameter
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

            int onlineUserId = 0;

            try
            {
                onlineUserId = _securityService.Login(loginParameter);
            }
            catch (Exception ex)
            {
                if (onlineUserId == 0)
                {
                    throw;
                }

                Logging.LogService.Error(ex.Message);
            }

            // saves user session
            this._currentSession.Level = this._userService.GetUserLevel(user.Id, this.SiteId);
            this._currentSession.UserId = user.Id;
            this._currentSession.Username = user.Username;
            this._currentSession.OnUserId = onlineUserId;
            this._currentSession.SessionId = Session.SessionID;
        }

        /// <summary>
        /// Checks the user permission on specific site.
        /// </summary>
        /// <param name="user">The user information.</param>
        /// <returns>If an user has permission on a specific site or not.</returns>
        private bool CheckUserSitePermission(User user)
        {
            var group = user.Group == null ? null : _groupService.Get(user.Group.Id);
            var siteLevelList = _userService.GetSiteLevel(user.Id, Environment);

            return (group == null || group.IsActive) && siteLevelList.Any(x => x.Site.Id == SiteId && x.IsActive);
        }

        /// <summary>
        /// Does the sign out.
        /// </summary>
        private void DoSignOut()
        {
            _securityService.Logout(_currentSession.OnUserId);

            _currentSession.ClearAll();
            Session.Clear();

            Response.Cookies.Clear();

            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Adds the login token to cookie.
        /// </summary>
        /// <param name="loginToken">The login token.</param>
        private void AddTokenToCookie(string loginToken)
        {
            // Create the non-persistent secure form token cookie
            var responseCookie = new HttpCookie(LoginFormToken)
            {
                HttpOnly = true,
                Value = loginToken
            };

            // If we are using SSL, the cookie should be set to secure to
            // prevent it from being sent over HTTP connections
            if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
            {
                responseCookie.Secure = true;
            }

            // Add the cookie to the response
            Response.Cookies.Set(responseCookie);
        }

        /// <summary>
        /// Generates the token for login form.
        /// </summary>
        private void GenerateToken()
        {
            string loginToken = Guid.NewGuid().ToString("N");

            FormToken = loginToken;
            ViewBag.LoginToken = loginToken;
            AddTokenToCookie(loginToken);
        }

        /// <summary>
        /// Checks if the token is valid or not.
        /// </summary>
        /// <param name="loginToken">The login token.</param>
        /// <returns>Boolean value indicates if the login token is valid or not.</returns>
        private bool CheckValidToken(string loginToken)
        {
            return !string.IsNullOrWhiteSpace(loginToken)
                && FormToken == loginToken
                && Request.Cookies[LoginFormToken] != null
                && loginToken == Request.Cookies[LoginFormToken].Value;
        }

        /// <summary>
        /// Gets the hash value of input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Hash value of input.</returns>
        private string GetHash(string input)
        {
            var encrypted = string.Empty;

            if (!string.IsNullOrWhiteSpace(input))
            {
                var hashData = _hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

                encrypted = BitConverter.ToString(hashData).Replace("-", string.Empty).ToLowerInvariant();
            }

            return encrypted;
        }

        /// <summary>
        /// Clears the token.
        /// </summary>
        private void ClearToken()
        {
            Session.Remove(LoginFormToken);

            if (Request.Cookies[LoginFormToken] != null)
            {
                HttpCookie cookie = new HttpCookie(LoginFormToken);

                cookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(cookie);
            }
        }

        #endregion Helpers
    }
}