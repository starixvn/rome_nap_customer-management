// -----------------------------------------------------------------------
// <copyright file="TurnoverControlNotificationController.cs">
// Copyright (c). All rights reserved.
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
        private IMailingListService _mailingListService;

        public TurnoverControlNotificationController()
            : this(null)
        {
        }

        public TurnoverControlNotificationController(IMailingListService mailingListService)
        {
            _mailingListService = mailingListService ?? new MailingListService();
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverControlNotification, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var model = this._mailingListService.MailingList();
            ViewBag.HasHostPermission = this.HasRight(CMSFunctionKeys.TurnoverControlNotification, Constant.Policy.Host);
            ViewBag.UserGroupId = _mailingListService.UserGroupId();
            return View("~/Areas/NotificationSetting/Views/TurnoverControl/Index.cshtml", model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverControlNotification, Policy = Constant.Policy.Host)]
        public ActionResult UpdateMailingList(MailInfo mailInfo, bool isUpdate)
        {
            Result result = this._mailingListService.UpdateMailingList(mailInfo, isUpdate);
            var jsonResult = Json(result);
            return jsonResult;
        }
    }
}