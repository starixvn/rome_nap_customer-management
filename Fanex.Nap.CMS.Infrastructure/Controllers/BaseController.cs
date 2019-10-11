// --------------------------------------------------------------------------
// <copyright file="BaseController.cs">
// Copyright (c). All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.Nap.CMS.Infrastructure.Controllers
{
    using System;
    using System.Web.Mvc;
    using Extensions;
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;
    using NAP.Core.Context;
    using Newtonsoft.Json;

    /// <summary>
    /// Base controller.
    /// </summary>
    public class BaseController : Controller
    {
        private readonly ICurrentSession _currentSession;

        private readonly ICurrentContext _currentContext;

        private readonly string _environment;

        private readonly int _siteId;

        private readonly bool _enableDirectLogin;

        private readonly IUserService _userService;

        public BaseController()
        {
            this._currentContext = new CurrentContext();
            this._currentSession = new CurrentSession();
            this._environment = NapSettings.Settings.Environment;
            this._siteId = NapSettings.Settings.SiteId;
            this._enableDirectLogin = NapSettings.Settings.EnableDirectLogin;

            ViewBag.Session = this._currentSession;

            ViewFlags = new ViewFlags
            {
                IsLive = this.Environment.Equals(Constant.LIVE),
                HasEditRight = false,
                IsReCheckin = NapSettings.Settings.EnableReCheckIn
            };

            ViewBag.ServerDate = ServerDate.ToShortFormatedDate();
            ViewBag.ShowMenus = SessionManager.Get<bool?>("ShowMenus");

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

        public ICurrentSession CurrentSession
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
            int functionPolicy = this._userService.GetPermission(this._currentSession.UserId, this.SiteId, functionKey);
            return functionPolicy >= (int)policy;
        }

        protected JsonResult JsonData(object data, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
        {
            var jsonString = JsonConvert.SerializeObject(
                data,
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            return Json(jsonString, behavior);
        }

        protected new JsonResult Json(object data, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
        {
            return new JsonResult
            {
                JsonRequestBehavior = behavior,
                Data = data,
                MaxJsonLength = int.MaxValue
            };
        }
    }
}