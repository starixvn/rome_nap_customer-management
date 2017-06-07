// --------------------------------------------------------------------------
// <copyright file="ConfigureTOByEventController.cs">
// Copyright (c). All rights reserved.
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
    using Fanex.NAP.CMS.Core;
    using Fanex.NAP.CMS.Criteria.Common;
    using Fanex.NAP.CMS.Criteria.ConfigureTOByEvent;
    using Fanex.NAP.CMS.Models.Common;
    using Fanex.NAP.CMS.Models.ConfigureTOByEvent;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.ConfigureTOByEvent;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Framework.Command;

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
            ViewBag.HasEditRight = this.HasRight(CMSFunctionKeys.ControlTOByEvent, Constant.Policy.Host);
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOByEvent, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets the sport turnover.
        /// </summary>
        /// <returns>List of JSON objects represents the sport turnovers.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOByEvent, Policy = Constant.Policy.Read)]
        public JsonResult GetSportTurnover()
        {
            IEnumerable<SportTurnover> result = this._configureTOByEventService.GetSportTurnover(new GetSportTurnoverCriteria());

            return Json(result);
        }

        /// <summary>
        /// Updates the sport turnover.
        /// </summary>
        /// <param name="filter">The filter that is used to update and query data.</param>
        /// <returns>List of JSON objects represents the sport turnovers after updating.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOByEvent, Policy = Constant.Policy.Host)]
        public JsonResult UpdateSportTurnover(UpdateSportTurnoverFilter filter)
        {
            var updatingCriteria = new UpdateSportTurnoverCriteria
            {
                SportID = filter.SportID,
                Value = filter.Value,
                UserID = this.CurrentSession.UserId,
                IP = this.CurrentContext.IP
            };
            ICommandResult updatingResult = this._configureTOByEventService.UpdateTurnover(updatingCriteria);
            IEnumerable<SportTurnover> result = null;

            // Get up-to-date data after updating.
            if (updatingResult.Code == 0)
            {
                result = this._configureTOByEventService.GetSportTurnover(new GetSportTurnoverCriteria());
            }

            return Json(new UpdatedTurnoverResult
            {
                ErrorCode = updatingResult.Code,
                ErrorMessage = updatingResult.Message,
                Turnovers = result
            });
        }

        /// <summary>
        /// Gets the sport turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of JSON objects represents the sport turnovers.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOByEvent, Policy = Constant.Policy.Read)]
        public JsonResult GetLeagueTurnover(GetLeagueTurnoverCriteria criteria)
        {
            GotLeagueTurnoverResult result = null;

            criteria.CountryCode = criteria.CountryCode ?? string.Empty;
            criteria.ProgramID = criteria.ProgramID ?? string.Empty;
            criteria.LeagueName = criteria.LeagueName ?? string.Empty;
            result = this._configureTOByEventService.GetLeagueTurnover(criteria);

            return Json(result);
        }

        /// <summary>
        /// Updates the league turnover.
        /// </summary>
        /// <param name="filter">The filter that is used to update and query data.</param>
        /// <returns>List of JSON objects represents the league turnovers after updating.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOByEvent, Policy = Constant.Policy.Host)]
        public JsonResult UpdateLeagueTurnover(UpdateLeagueTurnoverFilter filter)
        {
            var updatingCriteria = new UpdateLeagueTurnoverCriteria
            {
                LeagueID = filter.LeagueID,
                Value = filter.Value,
                UserID = this.CurrentSession.UserId,
                IP = this.CurrentContext.IP
            };
            var gettingCriteria = new GetLeagueTurnoverCriteria
            {
                SportID = filter.SportID,
                DisplayMode = filter.DisplayMode,
                ProgramID = filter.ProgramID,
                CountryCode = filter.CountryCode,
                AllMarket = filter.AllMarket,
                LeagueName = filter.LeagueName,
                OnlyShowException = filter.OnlyShowException
            };
            ICommandResult updatingResult = this._configureTOByEventService.UpdateTurnover(updatingCriteria);
            GotLeagueTurnoverResult turnovers = null;

            // Get up-to-date data after updating.
            if (updatingResult.Code == 0)
            {
                gettingCriteria.CountryCode = gettingCriteria.CountryCode ?? string.Empty;
                gettingCriteria.ProgramID = gettingCriteria.ProgramID ?? string.Empty;
                gettingCriteria.LeagueName = gettingCriteria.LeagueName ?? string.Empty;
                turnovers = this._configureTOByEventService.GetLeagueTurnover(gettingCriteria);
            }

            return Json(new UpdatedTurnoverResult
            {
                ErrorCode = updatingResult.Code,
                ErrorMessage = updatingResult.Message,
                Turnovers = turnovers
            });
        }

        /// <summary>
        /// Removes league exception.
        /// </summary>
        /// <param name="filter">The filter that is used to update and query data.</param>
        /// <returns>List of JSON objects represents the league turnovers after updating.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOByEvent, Policy = Constant.Policy.Host)]
        public JsonResult RemoveLeagueException(RemoveLeagueExceptionFilter filter)
        {
            var deletingCriteria = new RemoveLeagueExceptionCriteria
            {
                LeagueID = filter.LeagueID,
                UserID = this.CurrentSession.UserId,
                IP = this.CurrentContext.IP
            };
            var gettingCriteria = new GetLeagueTurnoverCriteria
            {
                SportID = filter.SportID,
                DisplayMode = filter.DisplayMode,
                ProgramID = filter.ProgramID,
                CountryCode = filter.CountryCode,
                AllMarket = filter.AllMarket,
                LeagueName = filter.LeagueName,
                OnlyShowException = filter.OnlyShowException
            };
            ICommandResult updatingResult = this._configureTOByEventService.UpdateTurnover(deletingCriteria);
            GotLeagueTurnoverResult turnovers = null;

            // Get up-to-date data after updating.
            if (updatingResult.Code == 0)
            {
                gettingCriteria.CountryCode = gettingCriteria.CountryCode ?? string.Empty;
                gettingCriteria.ProgramID = gettingCriteria.ProgramID ?? string.Empty;
                gettingCriteria.LeagueName = gettingCriteria.LeagueName ?? string.Empty;
                turnovers = this._configureTOByEventService.GetLeagueTurnover(gettingCriteria);
            }

            return Json(new UpdatedTurnoverResult
            {
                ErrorCode = updatingResult.Code,
                ErrorMessage = updatingResult.Message,
                Turnovers = turnovers
            });
        }

        /// <summary>
        /// Gets the match turnover.
        /// </summary>
        /// <param name="criteria">The criteria that are used to query data.</param>
        /// <returns>List of JSON objects represents the match turnovers.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOByEvent, Policy = Constant.Policy.Read)]
        public JsonResult GetMatchTurnover(GetMatchTurnoverCriteria criteria)
        {
            GotMatchTurnoverResult result = null;

            criteria.MatchCode = criteria.MatchCode ?? string.Empty;
            result = this._configureTOByEventService.GetMatchTurnover(criteria);

            return Json(result);
        }

        /// <summary>
        /// Updates the match turnover.
        /// </summary>
        /// <param name="filter">The filter that is used to update and query data.</param>
        /// <returns>List of JSON objects represents the match turnovers after updating.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOByEvent, Policy = Constant.Policy.Host)]
        public JsonResult UpdateMatchTurnover(UpdateMatchTurnoverFilter filter)
        {
            var updatingCriteria = new UpdateMatchTurnoverCriteria
            {
                MatchID = filter.MatchID,
                Value = filter.Value,
                UserID = this.CurrentSession.UserId,
                IP = this.CurrentContext.IP
            };
            var gettingCriteria = new GetMatchTurnoverCriteria
            {
                SportID = filter.SportID,
                LeagueID = filter.LeagueID,
                MatchCode = filter.MatchCode,
                OnlyShowException = filter.OnlyShowException
            };
            ICommandResult updatingResult = this._configureTOByEventService.UpdateTurnover(updatingCriteria);
            GotMatchTurnoverResult turnovers = null;

            // Get up-to-date data after updating.
            if (updatingResult.Code == 0)
            {
                gettingCriteria.MatchCode = gettingCriteria.MatchCode ?? string.Empty;
                turnovers = this._configureTOByEventService.GetMatchTurnover(gettingCriteria);
            }

            return Json(new UpdatedTurnoverResult
            {
                ErrorCode = updatingResult.Code,
                ErrorMessage = updatingResult.Message,
                Turnovers = turnovers
            });
        }

        /// <summary>
        /// Removes match exception.
        /// </summary>
        /// <param name="filter">The filter that is used to update and query data.</param>
        /// <returns>List of JSON objects represents the match turnovers after updating.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOByEvent, Policy = Constant.Policy.Host)]
        public JsonResult RemoveMatchException(RemoveMatchExceptionFilter filter)
        {
            var removeCriteria = new RemoveMatchExceptionCriteria
            {
                MatchID = filter.MatchID,
                UserID = this.CurrentSession.UserId,
                IP = this.CurrentContext.IP
            };
            var gettingCriteria = new GetMatchTurnoverCriteria
            {
                SportID = filter.SportID,
                LeagueID = filter.LeagueID,
                MatchCode = filter.MatchCode,
                OnlyShowException = filter.OnlyShowException
            };
            ICommandResult updatingResult = this._configureTOByEventService.UpdateTurnover(removeCriteria);
            GotMatchTurnoverResult turnovers = null;

            // Get up-to-date data after updating.
            if (updatingResult.Code == 0)
            {
                gettingCriteria.MatchCode = gettingCriteria.MatchCode ?? string.Empty;
                turnovers = this._configureTOByEventService.GetMatchTurnover(gettingCriteria);
            }

            return Json(new UpdatedTurnoverResult
            {
                ErrorCode = updatingResult.Code,
                ErrorMessage = updatingResult.Message,
                Turnovers = turnovers
            });
        }

        /// <summary>
        /// Get initialized data for Match page.
        /// </summary>
        /// <param name="mode">View mode.</param>
        /// <returns>List of JSON objects represents the sport list and league list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOByEvent, Policy = Constant.Policy.Read)]
        public JsonResult GetInitializedMatchData(byte mode = 0)
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
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOByEvent, Policy = Constant.Policy.Read)]
        public JsonResult GetLeaguesOfSport(GetLeaguesOfSportCriteria criteria)
        {
            IEnumerable<LeagueInfo> leagues = this._configureTOByEventService.GetLeaguesOfSport(criteria);

            return Json(leagues);
        }

        /// <summary>
        /// Get initialized data for League page.
        /// </summary>
        /// <param name="mode">View mode.</param>
        /// <returns>List of JSON objects represents the sport list and country list, and special league list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOByEvent, Policy = Constant.Policy.Read)]
        public JsonResult GetInitializedLeagueData(byte mode = 1)
        {
            IEnumerable<SportInfo> sports = this._configureTOByEventService.GetSportList(new GetSportsCriteria
            {
                Mode = mode
            });
            IEnumerable<CountryInfo> countries = this._configureTOByEventService.GetCountryList();
            IEnumerable<SpecialLeagueInfo> specialLeagues = this._configureTOByEventService.GetSpecialLeagueList();

            return Json(new
            {
                Sports = sports,
                Countries = countries,
                SpecialLeagues = specialLeagues
            });
        }
    }
}