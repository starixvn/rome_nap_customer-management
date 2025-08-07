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
    using System.Net;
    using System.Web.Mvc;
    using Configurations;
    using Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Resources;
    using Fanex.NAP.Client.Helpers.Business;
    using Fanex.NAP.Client.Attributes;
    using Newtonsoft.Json;
    using Web;
    using Fanex.NAP.Client.Services;
    using Fanex.NAP.Client;
    using Fanex.NAP.Client.Helpers.Web;

    [NapSuperAuth]
    public class BaseController : Controller
    {
        private readonly IUserService userService;

        public BaseController()
        {
            CurrentContext = new CurrentContext();
            CurrentSession = NapClientManager.NapSession;
            Environment = NapClientManager.NapSettings.Environment;
            SiteId = NapClientManager.NapSettings.SiteId;
            EnableDirectLogin = NapClientManager.NapSettings.EnableDirectLogin;

            ViewBag.Session = CurrentContext;

            ViewFlags = new ViewFlags
            {
                IsLive = Environment.Equals(Constant.LIVE),
                HasEditRight = false,
                IsReCheckin = NapClientManager.NapSettings.EnableReCheckIn
            };

            ViewBag.ServerDate = ServerDate.ToShortFormatedDate();
            ViewBag.ShowMenus = SessionManager.Get<bool?>("ShowMenus");
            ViewBag.Enviroment = AppSettings.Environment;

            userService = new UserService();
        }

        /// <summary>
        /// Gets a value indicating whether the current site allows users login directly.
        /// If "0", users must login through NAP Hub.
        /// </summary>
        /// <value>Is site login.</value>
        public bool EnableDirectLogin { get; }

        public ICurrentContext CurrentContext { get; }

        public INapSession CurrentSession { get; }

        public string Environment { get; }

        public int SiteId { get; }

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
                var serverTzi = TimeZoneInfo.FindSystemTimeZoneById(AppSettings.ServerTimezone);

                return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, serverTzi);
            }
        }

        public bool HasRight(string functionKey, Constant.Policy policy)
        {
            var functionPolicy = userService.GetPermission(CurrentSession.UserId, SiteId, functionKey);

            return functionPolicy >= (int)policy;
        }

        protected JsonResult BadRequest(string message = null, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
            => JsonData(new Result((int)HttpStatusCode.BadRequest, message ?? Labels.BadRequest), behavior);

        protected JsonResult Forbidden(string message = null, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
            => JsonData(new Result((int)HttpStatusCode.Forbidden, message ?? Labels.AccessDeniedMessage), behavior);

        protected JsonResult JsonData(object data, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
        {
            var jsonString = JsonConvert.SerializeObject(
                data,
                Formatting.None,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            return Json(jsonString, behavior);
        }

        protected new JsonResult Json(object data, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
            => new JsonResult
            {
                JsonRequestBehavior = behavior,
                Data = data,
                MaxJsonLength = int.MaxValue
            };

        protected ActionResult JsonNet(object data)
        {
            return Content(JsonConvert.SerializeObject(data), "application/json");
        }
    }
}