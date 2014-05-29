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
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.NAP.CMS.Core.Configurations;
    using Fanex.NAP.CMS.Criteria.Common;
    using Fanex.NAP.CMS.Criteria.TurnoverMonitor;
    using Fanex.NAP.CMS.Models.Common;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.TurnoverMonitor;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web;
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
            ViewBag.DefautPercentageUsed = AppSettings.TurnoverPercentageUsed.Format();

            return View();
        }

        /// <summary>
        /// Get initialized data for Match page.
        /// </summary>
        /// <returns>List of JSON objects represents the sport list and league list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOByEvent, Policy = Constant.Policy.Read)]
        public JsonResult GetInitializedData()
        {
            IEnumerable<SportInfo> sports = this._turnoverMonitorService.GetSportList(new GetSportsCriteria());
            IEnumerable<LeagueInfo> leagues = null;

            if (sports.Any())
            {
                leagues = this._turnoverMonitorService.GetLeaguesOfSport(new GetLeaguesOfSportCriteria
                {
                    SportID = sports.ElementAt<SportInfo>(0).SportType
                });
            }

            return Json(new
            {
                Sports = sports,
                Leagues = leagues
            });
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
        /// Gets the turnover report.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>JSON result indicates the turnover report data.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public JsonResult GetTurnoverReport(GetSMATurnoverMonitorListCriteria criteria)
        {
            int currentSportType = criteria.SportType;
            int currentLeagueID = criteria.LeagueID;
            SMATurnoverReport result = this._turnoverMonitorService.GetSMATurnoverMonitorList(criteria);
            IEnumerable<SportInfo> sports = this._turnoverMonitorService.GetSportList(new GetSportsCriteria());
            IEnumerable<LeagueInfo> leagues = null;

            if (sports.Any() && !sports.Any(obj => obj.SportType == criteria.SportType))
            {
                currentSportType = sports.ElementAt<SportInfo>(0).SportType;
            }

            leagues = this._turnoverMonitorService.GetLeaguesOfSport(new GetLeaguesOfSportCriteria
            {
                SportID = currentSportType
            });

            if (leagues.Any() && !leagues.Any(obj => obj.LeagueID == currentLeagueID))
            {
                currentLeagueID = 0;
            }

            return Json(new
            {
                Report = result,
                Sports = sports,
                Leagues = leagues,
                SportType = currentSportType,
                LeagueID = currentLeagueID
            });
        }
    }
}