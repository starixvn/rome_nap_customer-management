// --------------------------------------------------------------------------
// <copyright file="ConfigureTOByEventController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.4.16">Created</change>
// </history>

namespace Fanex.NAP.CMS.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.NAP.CMS.Criteria.ConfigureTOByEvent;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.ConfigureTOByEvent;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Configure turnover by event controller.
    /// </summary>
    public class ConfigureTOByEventController : BaseController
    {
        private const int LeagueMode = 2;
        private const int MatchMode = 3;
        private readonly IConfigureTOByEventService _configureTOByEventService;

        public ConfigureTOByEventController() :
            this(null)
        {
        }

        public ConfigureTOByEventController(IConfigureTOByEventService configureTOByEventService)
        {
            this._configureTOByEventService = configureTOByEventService ?? new ConfigureTOByEventService();
            ViewBag.HasEditRight = this.HasRight(CMSFunctionKeys.ConfigureTOByEvent, Constant.Policy.Host);
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.ConfigureTOByEvent, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets the sport turnover.
        /// </summary>
        /// <returns>List of JSON objects represents the sport turnovers.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ConfigureTOByEvent, Policy = Constant.Policy.Read)]
        public JsonResult GetSportTurnover()
        {
            var criteria = new GetEventTurnoverCriteria();
            IEnumerable<SportTurnover> result = null;

            criteria.ViewMode = 1;
            result = this._configureTOByEventService.GetSportTurnover(criteria);

            return Json(result);
        }

        /// <summary>
        /// Gets the league turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of JSON objects represents the sport turnovers.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ConfigureTOByEvent, Policy = Constant.Policy.Read)]
        public JsonResult GetLeagueTurnover(GetEventTurnoverCriteria criteria)
        {
            IEnumerable<LeagueTurnover> result = null;

            criteria.ViewMode = LeagueMode;
            result = this._configureTOByEventService.GetLeagueTurnover(criteria);

            return Json(result);
        }
    }
}