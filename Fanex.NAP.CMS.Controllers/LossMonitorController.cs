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
    using Fanex.NAP.CMS.Models.LossMonitor;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.LossMonitor;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Turnover monitor controller.
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
            List<SMALossGroup> result = GetSampleSMALoss();

            return Json(result);
        }

        /// <summary>
        /// Gets the sample SMA loss.
        /// </summary>
        /// <returns>List of SMALossGroup objects.</returns>
        private List<SMALossGroup> GetSampleSMALoss()
        {
            SMALossInfo rawData = new SMALossInfo
            {
                Level = 1,
                LevelRange = "90% & About",
                CustID = 1,
                Username = "Test",
                RoleID = 4,
                Currency = "UUS",
                TotalBalance = -900000,
                MaxLoss = -1000000,
                LossPercentage = 95
            };
            SMALossItem item = new SMALossItem(rawData);
            SMALossGroup group = new SMALossGroup(rawData);
            List<SMALossGroup> result = new List<SMALossGroup>();

            group.SMALossItems.Add(item);
            result.Add(group);

            return result;
        }
    }
}