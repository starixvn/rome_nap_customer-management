using System.Linq;
using System.Net;
using System.Web.Mvc;
using Fanex.Nap.CMS.Infrastructure;
using Fanex.Nap.CMS.Infrastructure.Controllers;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Resources;
using Fanex.Nap.CMS.Infrastructure.Web;
using Fanex.Nap.CMS.TurnoverControl.Criteria;
using Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor;
using Fanex.Nap.CMS.TurnoverControl.Enums;
using Fanex.Nap.CMS.TurnoverControl.Services.TurnoverMonitor;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.Nap.CMS.TurnoverControl.Controllers
{
    public class TurnoverMonitorController : BaseController
    {
        private readonly ITurnoverMonitorService _turnoverMonitorService;

        public TurnoverMonitorController()
            : this(null)
        {
        }

        public TurnoverMonitorController(ITurnoverMonitorService turnoverMonitorService)
        {
            _turnoverMonitorService = turnoverMonitorService ?? new TurnoverMonitorService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            AssignGlobalSportType();
            ViewData["HostPermission"] = HasRight(CMSFunctionKeys.TurnoverMonitor, Constant.Policy.Host);
            ViewData["SoccerHostPermission"] = HasRight(CMSFunctionKeys.TurnoverMonitorSoccer, Constant.Policy.Host);
            ViewData["OtherSportsHostPermission"] = HasRight(CMSFunctionKeys.TurnoverMonitorOtherSports, Constant.Policy.Host);

            return View("~/Areas/TurnoverControl/Views/TurnoverMonitor/Index.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public JsonResult GetSports(GetSportsCriteria criteria)
        {
            var result = _turnoverMonitorService.GetSports(criteria);

            return JsonData(result);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public JsonResult GetLeagues(GetLeaguesCriteria criteria)
        {
            criteria.MoveBetsDay = AppSession.BalanceUpDate.MoveBetsDay;
            var result = _turnoverMonitorService.GetLeagues(criteria);

            return JsonData(result);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public JsonResult GetMatches(GetMatchesCriteria criteria)
        {
            var result = _turnoverMonitorService.GetMatches(criteria);

            return JsonData(result);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public JsonResult GetCustomerTurnovers(GetCustomerTurnoversCriteria criteria)
        {
            criteria.MoveBetsDay = AppSession.BalanceUpDate.MoveBetsDay;
            var result = _turnoverMonitorService.GetCustomerTurnovers(criteria);

            return JsonData(result);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public ActionResult GetHouseTurnovers(GetHouseTurnoverCriteria criteria)
        {
            var result = _turnoverMonitorService.GetHouseTurnovers(criteria);

            return JsonData(result);
        }

        [HttpPost]
        public JsonResult UpdateMaxTurnover(UpdateMaxTurnoverCriteria criteria)
        {
            Result result;

            if (!criteria.IsValid())
            {
                result = new Result(400, Labels.BadRequest);
            }
            else if (!HasHostTurnoverMonitor(criteria.SportType))
            {
                result = new Result((int)HttpStatusCode.Forbidden, Labels.AccessDeniedMessage);
            }
            else
            {
                AssignUserIdAndIP(criteria);
                result = _turnoverMonitorService.UpdateMaxTurnover(criteria);
            }

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult UpdateMultipleMaxTurnover(UpdateMultipleMaxTurnoverCriteria criteria)
        {
            Result result;

            if (!criteria.IsValid())
            {
                result = new Result(400, Labels.BadRequest);
            }
            else if (!HasHostTurnoverMonitor(criteria.GlobalSettingItems.First().SportType))
            {
                result = new Result((int)HttpStatusCode.Forbidden, Labels.AccessDeniedMessage);
            }
            else
            {
                AssignUserIdAndIP(criteria);
                result = _turnoverMonitorService.UpdateMultipleMaxTurnover(criteria);
            }

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NonAction]
        private void AssignUserIdAndIP(NonQueryCriteria criteria)
        {
            criteria.UserId = ServiceContext.UserId;
            criteria.IP = ServiceContext.Ip;
        }

        [NonAction]
        private void AssignGlobalSportType()
        {
            short globalSportType;
            bool soccer = HasRight(CMSFunctionKeys.TurnoverMonitorSoccer, Constant.Policy.Read);
            bool otherSports = HasRight(CMSFunctionKeys.TurnoverMonitorOtherSports, Constant.Policy.Read);

            if (soccer && otherSports)
            {
                globalSportType = 0;
            }
            else if (soccer)
            {
                globalSportType = (short)SportType.Soccer;
            }
            else if (otherSports)
            {
                globalSportType = (short)SportType.Others;
            }
            else
            {
                globalSportType = -1;
            }

            ViewData["GlobalSportType"] = globalSportType;
        }

        private bool HasHostTurnoverMonitor(short sporttype)
        {
            string permissionKey = sporttype == (short)SportType.Soccer ?
                CMSFunctionKeys.TurnoverMonitorSoccer :
                CMSFunctionKeys.TurnoverMonitorOtherSports;

            return HasRight(permissionKey, Constant.Policy.Host);
        }
    }
}
