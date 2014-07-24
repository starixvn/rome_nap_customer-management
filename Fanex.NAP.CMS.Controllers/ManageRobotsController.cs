// --------------------------------------------------------------------------
// <copyright file="ManageRobotsController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.30">Created</change>
// </history>

namespace Fanex.NAP.CMS.Controllers
{
    using System.Web.Mvc;
    using Fanex.NAP.CMS.Core;
    using Fanex.NAP.CMS.Criteria.ManageRobots;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.ManageRobots;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Manage robots controller.
    /// </summary>
    public class ManageRobotsController : BaseController
    {
        private readonly IManageRobotsService _manageRobotService;

        public ManageRobotsController() :
            this(null)
        {
        }

        public ManageRobotsController(IManageRobotsService manageRobotService)
        {
            this._manageRobotService = manageRobotService ?? new ManageRobotsService();
            ViewBag.HasEditAccessRight = this.HasRight(CMSFunctionKeys.ManageRobots, Constant.Policy.Host);
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageRobots, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets robot list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to queried data.</param>
        /// <returns>Robot list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageRobots, Policy = Constant.Policy.Read)]
        public JsonResult GetRobotList(GetRobotAccountsCriteria criteria)
        {
            GettingRobotAccountResult result = null;

            criteria.Username = criteria.Username ?? string.Empty;
            result = this._manageRobotService.GetRobotAccounts(criteria);

            return Json(result);
        }

        /// <summary>
        /// Updates robot information and gets up-to-date robot list.
        /// </summary>
        /// <param name="filter">Filter conditions.</param>
        /// <returns>Robot list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageRobots, Policy = Constant.Policy.Host)]
        public JsonResult EditRobotInfo(UpdateRobotInfoFilter filter)
        {
            GettingRobotAccountResult robotList = new GettingRobotAccountResult();

            filter.SearchedUsername = filter.SearchedUsername ?? string.Empty;
            filter.UserID = this.CurrentSession.UserId;
            filter.IP = this.CurrentContext.IP;
            robotList = this._manageRobotService.UpdateRobotInfo(filter);

            return Json(robotList);
        }

        /// <summary>
        /// Adds new robot information and gets up-to-date robot list.
        /// </summary>
        /// <param name="filter">Filter conditions.</param>
        /// <returns>Robot list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageRobots, Policy = Constant.Policy.Host)]
        public JsonResult AddRobotAccount(AddRobotInfoFilter filter)
        {
            GettingRobotAccountResult robotList = new GettingRobotAccountResult();

            filter.SearchedUsername = filter.SearchedUsername ?? string.Empty;
            filter.UserID = this.CurrentSession.UserId;
            filter.IP = this.CurrentContext.IP;
            robotList = this._manageRobotService.AddRobotAccount(filter);

            return Json(robotList);
        }

        /// <summary>
        /// Updates robot status and gets up-to-date robot list.
        /// </summary>
        /// <param name="filter">Filter conditions.</param>
        /// <returns>Robot list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageRobots, Policy = Constant.Policy.Host)]
        public JsonResult UpdateRobotStatus(UpdateRobotStatusFilter filter)
        {
            GettingRobotAccountResult robotList = new GettingRobotAccountResult();

            filter.SearchedUsername = filter.SearchedUsername ?? string.Empty;
            filter.UserID = this.CurrentSession.UserId;
            filter.IP = this.CurrentContext.IP;
            robotList = this._manageRobotService.UpdateRobotStatus(filter);

            return Json(robotList);
        }
    }
}