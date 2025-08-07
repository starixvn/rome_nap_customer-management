// --------------------------------------------------------------------------
// <copyright file="BaseController.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.3.3">Created</change>
// </history>

namespace Fanex.NAP.CMS.Controllers
{
    using System;
    using System.Web.Mvc;
    using Core.Helpers;
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.Client;
    using Fanex.NAP.Client.Attributes;
    using Fanex.NAP.Client.Helpers.Business;
    using Fanex.NAP.Client.Helpers.Web;
    using Fanex.NAP.Client.Services;
    using Fanex.NAP.CMS.Core.Web;

    [NapSuperAuth]
    public class BaseController : Controller
    {
        private readonly ICurrentContext _currentContext;

        private readonly INapSession _currentSession;

        private readonly string _environment;

        private readonly int _siteId;

        private readonly bool _enableDirectLogin;

        private readonly IUserService _userService;

        public BaseController()
        {
            this._currentContext = new CurrentContext();
            this._currentSession = NapClientManager.NapSession;
            this._environment = NapClientManager.NapSettings.Environment;
            this._siteId = NapClientManager.NapSettings.SiteId;
            this._enableDirectLogin = NapClientManager.NapSettings.EnableDirectLogin;

            ViewBag.Session = this.CurrentSession;

            ViewFlags = new ViewFlags
            {
                IsLive = this.Environment.Equals(Constant.LIVE),
                HasEditRight = false,
                IsReCheckin = NapClientManager.NapSettings.EnableReCheckIn
            };

            ViewBag.ServerDate = ServerDate.ToShortFormatedDate();
            ViewBag.ShowMenus = SessionManager.Get<bool?>("ShowMenus");
            ViewBag.Enviroment = NapClientManager.NapSettings.Environment;

            this._userService = new UserService();
        }

        /// <summary>
        /// Gets a value indicating whether the current site allows users login directly.
        /// If "0", users must login through NAP Hub.
        /// </summary>
        /// <value>Is site login.</value>
        public bool EnableDirectLogin
        {
            get
            {
                return this._enableDirectLogin;
            }
        }

        public ICurrentContext CurrentContext
        {
            get
            {
                return this._currentContext;
            }
        }

        public INapSession CurrentSession
        {
            get
            {
                return this._currentSession;
            }
        }

        public string Environment
        {
            get
            {
                return this._environment;
            }
        }

        public int SiteId
        {
            get
            {
                return this._siteId;
            }
        }

        protected ViewFlags ViewFlags
        {
            get
            {
                return ViewBag.ViewFlags;
            }

            set
            {
                ViewBag.ViewFlags = value;
            }
        }

        /// <summary>
        /// Gets the server date.
        /// </summary>
        /// <value>
        /// The GMT-4 date.
        /// </value>
        protected static DateTime ServerDate
        {
            get
            {
                TimeZoneInfo serverTzi = TimeZoneInfo.FindSystemTimeZoneById(AppSettings.ServerTimezone);

                return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, serverTzi);
            }
        }

        /// <summary>
        /// Determines whether the specified user has the right on a function.
        /// </summary>
        /// <param name="functionKey">The function key.</param>
        /// <param name="policy">The policy.</param>
        /// <returns>Whether the specified user has the right on a function.</returns>
        public bool HasRight(string functionKey, Constant.Policy policy)
        {
            int functionPolicy = this._userService.GetPermission(this.CurrentSession.UserId, this.SiteId, functionKey);
            return functionPolicy >= (int)policy;
        }
    }
}