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
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.NAP.CMS.Criteria.ManageSmartPunters;
    using Fanex.NAP.CMS.Models.ManageSmartPunters;
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
        [NapAuth(FunctionKey = CMSFunctionKeys.ManageRobots, Policy = Constant.Policy.Read)]
        public JsonResult GetSmartPunterList(GetSmartPuntersCriteria criteria)
        {
            IEnumerable<SmartPunterItem> result = null;

            criteria.Username = criteria.Username ?? string.Empty;
            result = this.GenerateTestingData();

            return Json(new
            {
                SmartPunters = result,
                TotalItem = 1
            });
        }

        /// <summary>
        /// Generates the testing data.
        /// </summary>
        /// <returns>List of SmartPunterItem object.</returns>
        private IEnumerable<SmartPunterItem> GenerateTestingData()
        {
            List<SmartPunterItem> data = new List<SmartPunterItem>();

            data.Add(new SmartPunterItem(new SmartPunterInfo
                {
                    CustID = 1,
                    Username = "Test",
                    Nickname = "test",
                    StatusID = 1,
                    Currency = "UUS",
                    Site = "12Bet",
                    Super = "Supertest",
                    Master = "MasterTest",
                    Agent = "Agenttest",
                    Creator = "sa",
                    CreateDate = DateTime.Now,
                    Remark = "JS Test"
                }));

            return data;
        }
    }
}