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
    using Fanex.NAP.CMS.Core.Configurations;
    using Fanex.NAP.CMS.Core.Web;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// Base controller.
    /// </summary>
    public class BaseController : Controller
    {
        private readonly ICurrentContext _currentContext;

        private readonly ICurrentSession _currentSession;

        private readonly IServiceContext _serviceContext;

        private readonly string _environment;

        private readonly int _siteId;

        private readonly bool _enableDirectLogin;

        private readonly IUserService _userService;

        public BaseController()
        {
            this._currentContext = new CurrentContext();
            this._currentSession = new CurrentSession();
            this._serviceContext = new ServiceContext
            {
                UserId = this.CurrentSession.UserId,
                SiteId = this.SiteId,
                Ip = this.CurrentContext.IP
            };
            this._environment = NapSettings.Settings.Environment;
            this._siteId = NapSettings.Settings.SiteId;
            this._enableDirectLogin = NapSettings.Settings.EnableDirectLogin;

            ViewBag.Session = this.CurrentSession;

            ViewFlags = new ViewFlags
            {
                IsLive = this.Environment.Equals(Constant.LIVE),
                HasEditRight = false,
                IsReCheckin = NapSettings.Settings.EnableReCheckIn
            };

            ViewBag.ServerDate = ServerDate.ToShortFormatedDate();
            ViewBag.ShowMenus = SessionManager.Get<bool?>("ShowMenus");

            this._userService = new UserService { ServiceContext = this.ServiceContext };
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

        public ICurrentSession CurrentSession
        {
            get
            {
                return this._currentSession;
            }
        }

        public IServiceContext ServiceContext
        {
            get
            {
                return this._serviceContext;
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
            int functionPolicy = this._userService.GetUserSiteFunctionKeyPolicy(this.CurrentSession.UserId, this.SiteId, functionKey);
            return functionPolicy >= (int)policy;
        }
    }
}