// -----------------------------------------------------------------------
// <copyright file="TurnoverControlNotificationController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Nap.CMS.NotificationSetting.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.NotificationSetting.Model.TurnoverControlNotification;
    using Fanex.Nap.CMS.NotificationSetting.Services.TurnoverControlNotification;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TurnoverControlNotificationController : BaseController
    {
        private readonly IMailingListService _service;

        public TurnoverControlNotificationController()
            : this(null)
        {
        }

        public TurnoverControlNotificationController(IMailingListService service)
        {
            _service = service ?? new MailingListService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverControlNotification, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var model = this._service.MailingList();
            ViewBag.HasHostPermission = this.HasRight(CMSFunctionKeys.TurnoverControlNotification, Constant.Policy.Host);
            ViewBag.UserGroupId = _service.UserGroupId();
            return View("~/Areas/NotificationSetting/Views/TurnoverControl/Index.cshtml", model);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverControlNotification, Policy = Constant.Policy.Host)]
        public ActionResult UpdateMailingList(MailInfo mailInfo, bool isUpdate)
        {
            Result result = this._service.UpdateMailingList(mailInfo, isUpdate);
            var jsonResult = Json(result);
            return jsonResult;
        }
    }
}