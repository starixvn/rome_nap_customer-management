using System.Linq;
using System.Web.Mvc;
using Fanex.Nap.CMS.Infrastructure;
using Fanex.Nap.CMS.Infrastructure.Controllers;
using Fanex.Nap.CMS.Infrastructure.Web;
using Fanex.Nap.CMS.TurnoverControl.Constants;
using Fanex.Nap.CMS.TurnoverControl.Models;
using Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Services.TurnoverMonitor;
using Fanex.Nap.CMS.TurnoverControl.ViewModels.TurnoverMonitor;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.Nap.CMS.TurnoverControl.Controllers
{
    public class TurnoverMonitorController : BaseController
    {
        private readonly ITurnoverMonitorService turnoverMonitorService;

        public TurnoverMonitorController()
            : this(null, null)
        {
        }

        public TurnoverMonitorController(
            ITurnoverMonitorService turnoverMonitorService,
            IGlobalSettingService globalSettingService)
        {
            this.turnoverMonitorService = turnoverMonitorService ?? new TurnoverMonitorService();
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
        public JsonResult GetSports(SportViewModel viewModel)
            => JsonData(turnoverMonitorService.GetSports(viewModel.Mode, viewModel.SportType));

        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public JsonResult GetLeagues(LeagueViewModel viewModel)
            => JsonData(turnoverMonitorService.GetLeagues(
                viewModel.FromDate,
                viewModel.ToDate,
                viewModel.SportType,
                AppSession.BalanceUpDate.MoveBetsDay));

        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public JsonResult GetMatches(MatchViewModel viewModel)
            => JsonData(turnoverMonitorService.GetMatches(viewModel.FromDate, viewModel.ToDate, viewModel.LeagueId));

        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public JsonResult GetCustomerTurnovers(CustomerTurnoverViewModel viewModel)
            => JsonData(turnoverMonitorService.GetCustomerTurnovers(
                viewModel.FromDate,
                viewModel.ToDate,
                viewModel.SportType,
                viewModel.LeagueId,
                viewModel.MatchId,
                viewModel.Username,
                viewModel.IsExcludeCompletedEvent,
                viewModel.PercentageUsed,
                AppSession.BalanceUpDate.MoveBetsDay));

        [NapAuth(FunctionKey = CMSFunctionKeys.TurnoverMonitor, Policy = Constant.Policy.Read)]
        public ActionResult GetHouseTurnovers(HouseTurnoverViewModel viewModel)
            => JsonData(turnoverMonitorService.GetHouseTurnovers(
                viewModel.CustId,
                viewModel.MatchId,
                viewModel.IsGroupHeadline,
                viewModel.EventDate));

        [HttpPost]
        public JsonResult UpdateMaxTurnover(UpdateMaxTurnoverViewModel viewModel)
        {
            if (!viewModel.IsValid())
            {
                return BadRequest();
            }

            if (!HasHostTurnoverMonitor(viewModel.SportType))
            {
                return Forbidden();
            }

            var param = new UpdateMaxTurnoverParam(
                viewModel.CustId,
                viewModel.MaxTurnover,
                viewModel.IsGroupHeadline,
                viewModel.SportType,
                viewModel.ProfileId,
                viewModel.MatchId);
            var result = turnoverMonitorService.UpdateMaxTurnover(param, CurrentSession.UserId, CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult UpdateMultipleMaxTurnover(UpdateMaxTurnoverViewModel[] globalSettings)
        {
            var firstItem = globalSettings?.FirstOrDefault();

            if (firstItem == null)
            {
                return BadRequest();
            }

            if (!HasHostTurnoverMonitor(firstItem.SportType))
            {
                return Forbidden();
            }

            var param = globalSettings.Select(vm => new UpdateMaxTurnoverParam(
                vm.CustId,
                vm.MaxTurnover,
                vm.IsGroupHeadline,
                vm.SportType,
                vm.ProfileId,
                vm.MatchId));
            var result = turnoverMonitorService.UpdateMultipleMaxTurnover(
                    param,
                    CurrentSession.UserId,
                    CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult Lock(LockEventSettingViewModel viewModel)
        {
            if (!viewModel.IsValid())
            {
                return BadRequest();
            }

            if (!HasHostTurnoverMonitor(viewModel.SportType))
            {
                return Forbidden();
            }

            var param = new LockSettingParam(
                viewModel.CustId,
                viewModel.IsLock,
                viewModel.IsGroupHeadline,
                viewModel.SportType,
                viewModel.MatchId);
            var result = turnoverMonitorService.Lock(param, CurrentSession.UserId, CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

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
                globalSportType = SportTypeConst.Soccer;
            }
            else if (otherSports)
            {
                globalSportType = SportTypeConst.Others;
            }
            else
            {
                globalSportType = -1;
            }

            ViewData["GlobalSportType"] = globalSportType;
        }

        private bool HasHostTurnoverMonitor(short sportType)
        {
            string permissionKey = sportType == SportTypeConst.Soccer
                ? CMSFunctionKeys.TurnoverMonitorSoccer
                : CMSFunctionKeys.TurnoverMonitorOtherSports;

            return HasRight(permissionKey, Constant.Policy.Host);
        }
    }
}
