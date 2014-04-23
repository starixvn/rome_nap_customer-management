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
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.NAP.CMS.Criteria.ConfigureTOByEvent;
    using Fanex.NAP.CMS.Models.ConfigureTOByEvent;
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
            IEnumerable<SportTurnover> result = this._configureTOByEventService.GetSportTurnover(new GetEventTurnoverCriteria());

            return Json(result);
        }

        /// <summary>
        /// Updates the sport turnover.
        /// </summary>
        /// <param name="fillter">The filter that is used to update and query data.</param>
        /// <returns>List of JSON objects represents the sport turnovers after updating.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ConfigureTOByEvent, Policy = Constant.Policy.Host)]
        public JsonResult UpdateSportTurnover(UpdateSportTurnoverFilter fillter)
        {
            UpdatedTurnoverResult result = null;

            fillter.UserID = this.CurrentSession.UserId;
            fillter.IP = this.CurrentContext.IP;
            result = this._configureTOByEventService.UpdateSportTurnover(fillter);

            return Json(result);
        }

        /// <summary>
        /// Gets the sport turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of JSON objects represents the sport turnovers.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ConfigureTOByEvent, Policy = Constant.Policy.Read)]
        public JsonResult GetLeagueTurnover(GetEventTurnoverCriteria criteria)
        {
            GotLeagueTurnoverResult result = this._configureTOByEventService.GetLeagueTurnover(criteria);

            return Json(result);
        }

        /// <summary>
        /// Updates the league turnover.
        /// </summary>
        /// <param name="fillter">The filter that is used to update and query data.</param>
        /// <returns>List of JSON objects represents the league turnovers after updating.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ConfigureTOByEvent, Policy = Constant.Policy.Host)]
        public JsonResult UpdateLeagueTurnover(UpdateLeagueTurnoverFilter fillter)
        {
            UpdatedTurnoverResult result = null;

            fillter.UserID = this.CurrentSession.UserId;
            fillter.IP = this.CurrentContext.IP;
            result = this._configureTOByEventService.UpdateLeagueTurnover(fillter);

            return Json(result);
        }

        /// <summary>
        /// Removes league exception.
        /// </summary>
        /// <param name="fillter">The filter that is used to update and query data.</param>
        /// <returns>List of JSON objects represents the league turnovers after updating.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ConfigureTOByEvent, Policy = Constant.Policy.Host)]
        public JsonResult RemoveLeagueException(RemoveLeagueExceptionFilter fillter)
        {
            UpdatedTurnoverResult result = null;

            fillter.UserID = this.CurrentSession.UserId;
            fillter.IP = this.CurrentContext.IP;
            result = this._configureTOByEventService.RemoveLeagueException(fillter);

            return Json(result);
        }

        /// <summary>
        /// Gets the match turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of JSON objects represents the match turnovers.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ConfigureTOByEvent, Policy = Constant.Policy.Read)]
        public JsonResult GetMatchTurnover(GetEventTurnoverCriteria criteria)
        {
            GotMatchTurnoverResult result = null;

            criteria.MatchCode = criteria.MatchCode ?? string.Empty;
            result = this._configureTOByEventService.GetMatchTurnover(criteria);

            return Json(result);
        }

        /// <summary>
        /// Updates the match turnover.
        /// </summary>
        /// <param name="fillter">The filter that is used to update and query data.</param>
        /// <returns>List of JSON objects represents the match turnovers after updating.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ConfigureTOByEvent, Policy = Constant.Policy.Host)]
        public JsonResult UpdateMatchTurnover(UpdateMatchTurnoverFilter fillter)
        {
            UpdatedTurnoverResult result = null;

            fillter.UserID = this.CurrentSession.UserId;
            fillter.IP = this.CurrentContext.IP;
            result = this._configureTOByEventService.UpdateMatchTurnover(fillter);

            return Json(result);
        }

        /// <summary>
        /// Removes match exception.
        /// </summary>
        /// <param name="fillter">The filter that is used to update and query data.</param>
        /// <returns>List of JSON objects represents the match turnovers after updating.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ConfigureTOByEvent, Policy = Constant.Policy.Host)]
        public JsonResult RemoveMatchException(RemoveMatchExceptionFilter fillter)
        {
            UpdatedTurnoverResult result = null;

            fillter.UserID = this.CurrentSession.UserId;
            fillter.IP = this.CurrentContext.IP;
            result = this._configureTOByEventService.RemoveMatchException(fillter);

            return Json(result);
        }

        /// <summary>
        /// Get initialized data for League page.
        /// </summary>
        /// <param name="mode">View mode.</param>
        /// <returns>List of JSON objects represents the sport list and league list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ConfigureTOByEvent, Policy = Constant.Policy.Read)]
        public JsonResult GetInitializedSportData(byte mode = 0)
        {
            IEnumerable<SportInfo> sports = this._configureTOByEventService.GetSportList(new GetSportsCriteria
            {
                Mode = mode
            });
            IEnumerable<LeagueInfo> leagues = null;

            if (sports.Any())
            {
                leagues = this._configureTOByEventService.GetLeaguesOfSport(new GetLeaguesOfSportCriteria
                {
                    SportID = sports.ElementAt<SportInfo>(0).SportType,
                    Mode = mode
                });
            }

            return Json(new
            {
                Sports = sports,
                Leagues = leagues
            });
        }

        /// <summary>
        /// Get the list of leagues of a specific sport.
        /// </summary>
        /// <param name="criteria">Criteria that are used to query data.</param>
        /// <returns>List of JSON objects represents the sport list and league list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ConfigureTOByEvent, Policy = Constant.Policy.Read)]
        public JsonResult GetLeaguesOfSport(GetLeaguesOfSportCriteria criteria)
        {
            IEnumerable<LeagueInfo> leagues = this._configureTOByEventService.GetLeaguesOfSport(criteria);

            return Json(leagues);
        }
    }
}