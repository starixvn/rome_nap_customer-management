// --------------------------------------------------------------------------
// <copyright file="TurnoverMonitorController.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.NAP.CMS.Criteria.Common;
    using Fanex.NAP.CMS.Models.Common;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.TurnoverMonitor;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Turnover monitor controller.
    /// </summary>
    public class TurnoverMonitorController : BaseController
    {
        private readonly ITurnoverMonitorService _turnoverMonitorService;

        public TurnoverMonitorController() :
            this(null)
        {
        }

        public TurnoverMonitorController(ITurnoverMonitorService turnoverMonitorService)
        {
            this._turnoverMonitorService = turnoverMonitorService ?? new TurnoverMonitorService();
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.SelectTop = GetSelectTopList();
            ViewBag.MoveBetsDay = (this._turnoverMonitorService as TurnoverMonitorService).BalanceUpDate.MoveBetsDay;

            return View();
        }

        /// <summary>
        /// Gets the list of leagues of a specific sport.
        /// </summary>
        /// <param name="criteria">Criteria that are used to query data.</param>
        /// <returns>List of JSON objects represents the list of leagues of a specific sport.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public JsonResult GetSportList(GetSportsCriteria criteria)
        {
            IEnumerable<SportInfo> sports = this._turnoverMonitorService.GetSportList(criteria);

            return Json(sports);
        }

        /// <summary>
        /// Gets the list of leagues of a specific sport.
        /// </summary>
        /// <param name="criteria">Criteria that are used to query data.</param>
        /// <returns>List of JSON objects represents the list of leagues of a specific sport.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public JsonResult GetLeaguesOfSport(GetLeaguesOfSportCriteria criteria)
        {
            IEnumerable<LeagueInfo> leagues = this._turnoverMonitorService.GetLeaguesOfSport(criteria);

            return Json(leagues);
        }

        /// <summary>
        /// Gets the list of matches of a specific league.
        /// </summary>
        /// <param name="criteria">Criteria that are used to query data.</param>
        /// <returns>List of JSON objects represents the list of matches of a specific league.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public JsonResult GetMatchesOfLeague(GetMatchesOfLeagueCriteria criteria)
        {
            IEnumerable<MatchInfo> matches = this._turnoverMonitorService.GetMatchesOfLeague(criteria);

            return Json(matches);
        }

        /// <summary>
        /// Gets the turnover report.
        /// </summary>
        /// <returns>JSON result indicates the turnover report data.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public JsonResult GetTurnoverReport()
        {
            var test = new SMATurnover
            {
                Username = "Supertest1",
                Level = "Super",
                Currency = "UUS",
                GivenCredit = 100000,
                MaxTORate = 90,
                MaxTOAmount = 90000,
                MemberTO = 63000,
                PercentageUsed = 70
            };
            var list = new List<SMATurnover>();

            list.Add(test);

            return Json(list);
        }

        /// <summary>
        /// Gets the select top list.
        /// </summary>
        /// <returns>List of SelectListItem objects.</returns>
        private IEnumerable<SelectListItem> GetSelectTopList()
        {
            string[] selectTopValues = new string[4] { "20", "50", "100", "200" };
            var selectTopOptions = new List<SelectListItem>();

            foreach (string option in selectTopValues)
            {
                selectTopOptions.Add(new SelectListItem
                    {
                        Text = option,
                        Value = option
                    });
            }

            return selectTopOptions;
        }
    }
}