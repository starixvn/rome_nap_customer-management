// --------------------------------------------------------------------------
// <copyright file="ManageSmartPuntersController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.6.4">Created</change>
// </history>

namespace Fanex.NAP.CMS.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.NAP.CMS.Criteria.ManageSmartPunters;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.ManageSmartPunters;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Manage smart punters controller.
    /// </summary>
    public class ManageSmartPuntersController : BaseController
    {
        private readonly IManageSmartPuntersService _manageSmartPuntersService;

        public ManageSmartPuntersController() :
            this(null)
        {
        }

        public ManageSmartPuntersController(IManageSmartPuntersService manageSmartPuntersService)
        {
            this._manageSmartPuntersService = manageSmartPuntersService ?? new ManageSmartPuntersService();
            ViewBag.HasEditAccessRight = this.HasRight(CMSFunctionKeys.ManageSmartPunters, Constant.Policy.Host);
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageSmartPunters, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets smart punter list.
        /// </summary>
        /// <param name="criteria">The criteria that are used to queried data.</param>
        /// <returns>Smart punter list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageSmartPunters, Policy = Constant.Policy.Read)]
        public JsonResult GetSmartPunterList(GetSmartPuntersCriteria criteria)
        {
            IEnumerable<SmartPunterItem> result = null;

            criteria.Username = criteria.Username ?? string.Empty;
            result = this._manageSmartPuntersService.GetSmartPunters(criteria);

            return Json(new
            {
                SmartPunters = result,
                TotalItems = criteria.TotalItems
            });
        }

        /// <summary>
        /// Adds new smart punter and gets up-to-date smart punter list.
        /// </summary>
        /// <param name="filter">Filter conditions.</param>
        /// <returns>Smart punter list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageSmartPunters, Policy = Constant.Policy.Host)]
        public JsonResult AddSmartPunter(AddSmartPunterFilter filter)
        {
            UpdatingSmartPunterResult smartPunters = null;

            filter.SearchedUsername = filter.SearchedUsername ?? string.Empty;
            filter.UserID = this.CurrentSession.UserId;
            filter.IP = this.CurrentContext.IP;
            smartPunters = this._manageSmartPuntersService.AddSmartPunter(filter);

            return Json(smartPunters);
        }

        /// <summary>
        /// Updates smart punter and gets up-to-date smart punter list.
        /// </summary>
        /// <param name="filter">Filter conditions.</param>
        /// <returns>Smart punter list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageSmartPunters, Policy = Constant.Policy.Host)]
        public JsonResult UpdateSmartPunter(UpdateSmartPunterFilter filter)
        {
            UpdatingSmartPunterResult smartPunters = new UpdatingSmartPunterResult();

            filter.SearchedUsername = filter.SearchedUsername ?? string.Empty;
            filter.UserID = this.CurrentSession.UserId;
            filter.IP = this.CurrentContext.IP;
            smartPunters = this._manageSmartPuntersService.UpdateSmartPunter(filter);

            return Json(smartPunters);
        }

        /// <summary>
        /// Deletes an existing smart punter and gets up-to-date smart punter list.
        /// </summary>
        /// <param name="filter">Filter conditions.</param>
        /// <returns>Smart punter list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageSmartPunters, Policy = Constant.Policy.Host)]
        public JsonResult RemoveSmartPunter(DeleteSmartPunterFilter filter)
        {
            UpdatingSmartPunterResult smartPunters = new UpdatingSmartPunterResult();

            filter.SearchedUsername = filter.SearchedUsername ?? string.Empty;
            filter.UserID = this.CurrentSession.UserId;
            filter.IP = this.CurrentContext.IP;
            smartPunters = this._manageSmartPuntersService.DeleteSmartPunter(filter);

            return Json(smartPunters);
        }
    }
}