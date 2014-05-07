// --------------------------------------------------------------------------
// <copyright file="TurnoverUsageController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2014.5.6">Created</change>
// </history>

namespace Fanex.NAP.CMS.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.NAP.CMS.Criteria.Common;
    using Fanex.NAP.CMS.Criteria.TurnoverUsage;
    using Fanex.NAP.CMS.Models.Common;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.TurnoverUsage;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Turnover usage controller.
    /// </summary>
    public class TurnoverUsageController : BaseController
    {
        private const byte MonitoredMode = 2;
        private readonly ITurnoverUsageService _turnoverUsageService;

        public TurnoverUsageController() :
            this(null)
        {
        }

        public TurnoverUsageController(ITurnoverUsageService turnoverUsageService)
        {
            this._turnoverUsageService = turnoverUsageService ?? new TurnoverUsageService();
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverUsage, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.MoveBetsDay = (this._turnoverUsageService as TurnoverUsageService).BalanceUpDate.MoveBetsDay;

            return View();
        }

        /// <summary>
        /// Gets the list of leagues of a specific sport.
        /// </summary>
        /// <param name="criteria">Criteria that are used to query data.</param>
        /// <returns>List of JSON objects represents the list of leagues of a specific sport.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverUsage, Policy = Constant.Policy.Read)]
        public JsonResult GetSportList(GetSportsCriteria criteria)
        {
            IEnumerable<SportInfo> sports = null;

            criteria.Mode = MonitoredMode;
            sports = this._turnoverUsageService.GetSportList(criteria);

            return Json(sports);
        }

        /// <summary>
        /// Gets the list of leagues of a specific sport.
        /// </summary>
        /// <param name="criteria">Criteria that are used to query data.</param>
        /// <returns>List of JSON objects represents the list of leagues of a specific sport.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverUsage, Policy = Constant.Policy.Read)]
        public JsonResult GetLeaguesOfSport(GetLeaguesOfSportCriteria criteria)
        {
            IEnumerable<LeagueInfo> leagues = null;

            criteria.Mode = MonitoredMode;
            leagues = this._turnoverUsageService.GetLeaguesOfSport(criteria);

            return Json(leagues);
        }

        /// <summary>
        /// Gets the list of matches of a specific league.
        /// </summary>
        /// <param name="criteria">Criteria that are used to query data.</param>
        /// <returns>List of JSON objects represents the list of matches of a specific league.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverUsage, Policy = Constant.Policy.Read)]
        public JsonResult GetMatchesOfLeague(GetMatchesOfLeagueCriteria criteria)
        {
            IEnumerable<MatchInfo> matches = this._turnoverUsageService.GetMatchesOfLeague(criteria);

            return Json(matches);
        }

        /// <summary>
        /// Gets the turnover report.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>JSON result indicates the turnover report data.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverUsage, Policy = Constant.Policy.Read)]
        public JsonResult GetTurnoverUsageReport(GetTurnoverUsageCriteria criteria)
        {
            IEnumerable<TurnoverUsageItem> list = this._turnoverUsageService.GetTurnoverUsageReport(criteria);

            return Json(list);
        }
    }
}