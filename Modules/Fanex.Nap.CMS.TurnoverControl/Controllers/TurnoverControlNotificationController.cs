// --------------------------------------------------------------------------
// <copyright file="TurnoverControlNotificationController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Turnover Control Notification Controller.
    /// </summary>
    public class TurnoverControlNotificationController : BaseController
    {
        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/TurnoverControl/Views/TurnoverMonitor/Index.cshtml");
        }
    }
}
