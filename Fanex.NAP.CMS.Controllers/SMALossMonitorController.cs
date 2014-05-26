// --------------------------------------------------------------------------
// <copyright file="SMALossMonitorController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.23">Created</change>
// </history>

namespace Fanex.NAP.CMS.Controllers
{
    using System.Web.Mvc;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Turnover monitor controller.
    /// </summary>
    public class SMALossMonitorController : BaseController
    {
        private readonly ISMALossMonitorService _lossMonitorService;

        public SMALossMonitorController() :
            this(null)
        {
        }

        public SMALossMonitorController(ISMALossMonitorService lossMonitorService)
        {
            this._lossMonitorService = lossMonitorService ?? new SMALossMonitorService();
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }
    }
}