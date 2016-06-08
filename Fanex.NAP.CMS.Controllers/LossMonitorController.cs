// --------------------------------------------------------------------------
// <copyright file="LossMonitorController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.23">Created</change>
// </history>

namespace Fanex.NAP.CMS.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.NAP.CMS.Core;
    using Fanex.NAP.CMS.Criteria.LossMonitor;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.LossMonitor;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Loss monitor controller.
    /// </summary>
    public class LossMonitorController : BaseController
    {
        private readonly ILossMonitorService _lossMonitorService;

        public LossMonitorController() :
            this(null)
        {
        }

        public LossMonitorController(ILossMonitorService lossMonitorService)
        {
            this._lossMonitorService = lossMonitorService ?? new LossMonitorService();
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.LossMonitor, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets SMA loss report.
        /// </summary>
        /// <returns>JSON result represents the SMA loss report.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.LossMonitor, Policy = Constant.Policy.Read)]
        public JsonResult GetSMALossReport()
        {
            List<SMALossGroup> result = this._lossMonitorService.GetSMALoss(new GetSMALossMonitorListCriteria());

            return Json(result);
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.LossMonitor, Policy = Constant.Policy.Host)]
        public JsonResult AddSMA(string username, int maxLoss)
        {
            var result = this._lossMonitorService.InsertSMA(new InsertSMACriteria
            {
                Username = username,
                MaxLoss = maxLoss,
                Creator = this.CurrentSession.UserId,
                IP = this.CurrentContext.IP
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.LossMonitor, Policy = Constant.Policy.Host)]
        public JsonResult DeleteSMA(int custId)
        {
            var result = this._lossMonitorService.DeleteSMA(new DeleteSMACriteria
            {
                CustId = custId,
                Creator = this.CurrentSession.UserId,
                IP = this.CurrentContext.IP
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.LossMonitor, Policy = Constant.Policy.Host)]
        public JsonResult EditMaxLoss(int custId, int maxLoss)
        {
            var result = this._lossMonitorService.UpdateMaxLoss(new UpdateSMAMaxLossCriteria
            {
                CustId = custId,
                MaxLoss = maxLoss,
                Creator = this.CurrentSession.UserId,
                IP = this.CurrentContext.IP
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}