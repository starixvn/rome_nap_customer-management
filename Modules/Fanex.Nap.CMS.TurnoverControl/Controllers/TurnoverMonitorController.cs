// --------------------------------------------------------------------------
// <copyright file="TurnoverMonitorController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Controllers
{
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor;
    using Fanex.Nap.CMS.TurnoverControl.Services.TurnoverMonitor;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Turnover Monitor Controller.
    /// </summary>
    public class TurnoverMonitorController : BaseController
    {
        private ITurnoverMonitorService _turnoverMonitorService;

        public TurnoverMonitorController()
            : this(null)
        {
        }

        public TurnoverMonitorController(ITurnoverMonitorService turnoverMonitorService)
        {
            _turnoverMonitorService = turnoverMonitorService ?? new TurnoverMonitorService();
        }

        /// <summary>
        /// Redirects to Index view.
        /// </summary>
        /// <returns>Index view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Areas/TurnoverControl/Views/TurnoverMonitor/Index.cshtml");
        }

        /// <summary>
        /// Gets sport list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// Sport list.
        /// </returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public JsonResult Sports(GetSportsCriteria criteria)
        {
            var result = _turnoverMonitorService.Sports(criteria);
            return Json(result);
        }

        /// <summary>
        /// Gets league list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// League list.
        /// </returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public JsonResult Leagues(GetLeaguesCriteria criteria)
        {
            var result = _turnoverMonitorService.Leagues(criteria);
            return Json(result);
        }

        /// <summary>
        /// Gets SMA turnover list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// SMA turnover list.
        /// </returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public JsonResult SMATurnoverList(GetSMATurnoverListCriteria criteria)
        {
            var result = _turnoverMonitorService.SMATurnoverList(criteria);
            return Json(result);
        }

        /// <summary>
        /// Redirects to Open Add New Setting view.
        /// </summary>
        /// <param name="custId">The customer identifier.</param>
        /// <param name="matchId">The match identifier.</param>
        /// <returns>
        /// Index view.
        /// </returns>
        [HttpGet]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public ActionResult OpenCompanyTurnoverView(int custId, int matchId)
        {
            var companyTurnover = _turnoverMonitorService.CompanyTurnover(new GetCompanyTurnoverCriteria
            {
                CustId = custId,
                MatchId = matchId
            });

            return View("~/Areas/TurnoverControl/Views/TurnoverMonitor/CompanyTurnover/Index.cshtml", companyTurnover);
        }

        /// <summary>
        /// Gets SMA turnover list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>
        /// SMA turnover list.
        /// </returns>
        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Host)]
        public JsonResult UpdateCustomerMaxTurnoverByMatch(UpdateMaxTurnoverByMatchCriteria criteria)
        {
            var result = _turnoverMonitorService.UpdateCustomerMaxTurnoverByMatch(criteria);
            return Json(result);
        }
    }
}
