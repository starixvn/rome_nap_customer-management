// --------------------------------------------------------------------------
// <copyright file="ConfigureSMATOByEventController.cs" company="Nexcel Solutions Vietnam">
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
    using Fanex.NAP.CMS.Core;
    using Fanex.NAP.CMS.Criteria.Common;
    using Fanex.NAP.CMS.Criteria.ConfigureSMATOByEvent;
    using Fanex.NAP.CMS.Models.Common;
    using Fanex.NAP.CMS.Services.Implements;
    using Fanex.NAP.CMS.Services.Interfaces;
    using Fanex.NAP.CMS.ViewModels.ConfigureSMATOByEvent;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Framework.Command;

    /// <summary>
    /// Configure turnover by event controller.
    /// </summary>
    public class ConfigureSMATOByEventController : BaseController
    {
        private readonly IConfigureSMATOByEventService _configureSMATOByMatchService;

        public ConfigureSMATOByEventController() :
            this(null)
        {
        }

        public ConfigureSMATOByEventController(IConfigureSMATOByEventService configureSMATOByMatchService)
        {
            this._configureSMATOByMatchService = configureSMATOByMatchService ?? new ConfigureSMATOByEventService();
            ViewBag.HasEditAccessRight = this.HasRight(CMSFunctionKeys.ControlTOBySMAEvent, Constant.Policy.Host);
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOBySMAEvent, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Gets initialized data for the page.
        /// </summary>
        /// <param name="mode">View mode.</param>
        /// <returns>List of JSON objects represents the sport list and league list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOBySMAEvent, Policy = Constant.Policy.Read)]
        public JsonResult GetInitializedData(byte mode = 0)
        {
            IEnumerable<SportInfo> sports = this._configureSMATOByMatchService.GetSportList(new GetSportsCriteria
            {
                Mode = mode
            });
            IEnumerable<LeagueInfo> leagues = null;

            if (sports.Any())
            {
                leagues = this._configureSMATOByMatchService.GetLeaguesOfSport(new GetLeaguesOfSportCriteria
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
        /// Gets the list of leagues of a specific sport.
        /// </summary>
        /// <param name="criteria">Criteria that are used to query data.</param>
        /// <returns>List of JSON objects represents the sport list and league list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOBySMAEvent, Policy = Constant.Policy.Read)]
        public JsonResult GetLeaguesOfSport(GetLeaguesOfSportCriteria criteria)
        {
            IEnumerable<LeagueInfo> leagues = this._configureSMATOByMatchService.GetLeaguesOfSport(criteria);

            return Json(leagues);
        }

        /// <summary>
        /// Gets SMA turnover by event.
        /// </summary>
        /// <param name="criteria">Criteria that are used to query data.</param>
        /// <returns>List of JSON objects represents the SMA turnover list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOBySMAEvent, Policy = Constant.Policy.Read)]
        public JsonResult GetSMATurnoverByEvent(GetSMATurnoverByEventCriteria criteria)
        {
            IEnumerable<SMATurnoverByEventGroup> result = this._configureSMATOByMatchService.GetSMATurnoverByEvent(criteria);

            return Json(result);
        }

        /// <summary>
        /// Updates SMA turnover by event.
        /// </summary>
        /// <param name="filter">Criteria contain the data that need to be updated.</param>
        /// <returns>List of JSON objects represents the SMA turnover list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOBySMAEvent, Policy = Constant.Policy.Host)]
        public JsonResult InsertSMATurnoverByEvent(InsertSMATurnoverByEventFilter filter)
        {
            var insertingCriteria = new InsertSMATurnoverByEventCriteria
            {
                MatchID = filter.MatchID,
                SMAName = filter.SMAName,
                Value = filter.Value,
                UserID = this.CurrentSession.UserId,
                IP = this.CurrentContext.IP
            };
            ICommandResult insertingResult = this._configureSMATOByMatchService.UpdateTurnover(insertingCriteria);
            IEnumerable<SMATurnoverByEventGroup> updatedResult = null;

            if (insertingResult.IsSuccessful)
            {
                var gettingCriteria = new GetSMATurnoverByEventCriteria
                {
                    SportID = filter.SportID,
                    LeagueID = filter.LeagueID
                };

                updatedResult = this._configureSMATOByMatchService.GetSMATurnoverByEvent(gettingCriteria);
            }

            return Json(new
            {
                ErrorCode = insertingResult.Code,
                ErrorMessage = insertingResult.Message,
                SMATurnovers = updatedResult
            });
        }

        /// <summary>
        /// Updates SMA turnover by event.
        /// </summary>
        /// <param name="filter">Criteria contain the data that need to be updated.</param>
        /// <returns>List of JSON objects represents the SMA turnover list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOBySMAEvent, Policy = Constant.Policy.Host)]
        public JsonResult UpdateSMATurnoverByEvent(UpdateSMATurnoverByEventFilter filter)
        {
            var updatingCriteria = new UpdateSMATurnoverByEventCriteria
            {
                MatchID = filter.MatchID,
                SMAID = filter.CustID,
                Value = filter.Value,
                UserID = this.CurrentSession.UserId,
                IP = this.CurrentContext.IP
            };
            ICommandResult updatingResult = this._configureSMATOByMatchService.UpdateTurnover(updatingCriteria);
            IEnumerable<SMATurnoverByEventGroup> updatedResult = null;

            if (updatingResult.IsSuccessful)
            {
                var gettingCriteria = new GetSMATurnoverByEventCriteria
                {
                    SportID = filter.SportID,
                    LeagueID = filter.LeagueID
                };

                updatedResult = this._configureSMATOByMatchService.GetSMATurnoverByEvent(gettingCriteria);
            }

            return Json(new
            {
                ErrorCode = updatingResult.Code,
                ErrorMessage = updatingResult.Message,
                SMATurnovers = updatedResult
            });
        }

        /// <summary>
        /// Removes SMA turnover by event.
        /// </summary>
        /// <param name="filter">Criteria contain the data that need to be removed from DB.</param>
        /// <returns>List of JSON objects represents the SMA turnover list.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.ControlTOBySMAEvent, Policy = Constant.Policy.Host)]
        public JsonResult RemoveSMATurnoverByEvent(RemoveSMATurnoverByEventFilter filter)
        {
            var deletingCriteria = new DeleteSMATurnoverByEventCriteria
            {
                MatchID = filter.MatchID,
                SMAID = filter.CustID,
                UserID = this.CurrentSession.UserId,
                IP = this.CurrentContext.IP
            };
            ICommandResult deletingResult = this._configureSMATOByMatchService.UpdateTurnover(deletingCriteria);
            IEnumerable<SMATurnoverByEventGroup> updatedResult = null;

            if (deletingResult.IsSuccessful)
            {
                var gettingCriteria = new GetSMATurnoverByEventCriteria
                {
                    SportID = filter.SportID,
                    LeagueID = filter.LeagueID
                };

                updatedResult = this._configureSMATOByMatchService.GetSMATurnoverByEvent(gettingCriteria);
            }

            return Json(new
            {
                ErrorCode = deletingResult.Code,
                ErrorMessage = deletingResult.Message,
                SMATurnovers = updatedResult
            });
        }
    }
}