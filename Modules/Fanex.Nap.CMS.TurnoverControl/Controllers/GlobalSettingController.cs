using System.Web.Mvc;
using Fanex.Nap.CMS.Infrastructure;
using Fanex.Nap.CMS.Infrastructure.Controllers;
using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
using Fanex.Nap.CMS.TurnoverControl.Constants;
using Fanex.Nap.CMS.TurnoverControl.Models;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.ViewModels.GlobalSetting;
using Fanex.NAP.Core.Business;

namespace Fanex.Nap.CMS.TurnoverControl.Controllers
{
    public class GlobalSettingController : BaseController
    {
        private readonly IGlobalSettingService globalSettingService;

        public GlobalSettingController()
            : this(null)
        {
        }

        public GlobalSettingController(IGlobalSettingService globalSettingService)
        {
            this.globalSettingService = globalSettingService ?? new GlobalSettingService();
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Index(short? t)
        {
            var sporttype = t ?? SportTypeConst.Soccer;

            if (!HasReadGlobalSetting(sporttype))
            {
                return Redirect("~/Error/AccessDenied");
            }

            var viewModel = new GlobalSettingViewModel(
                sporttype,
                HasHostGlobalSetting(sporttype),
                globalSettingService.GetCurrencies(),
                globalSettingService.GetProfiles(sporttype));

            return View("~/Areas/TurnoverControl/Views/GlobalSetting/Index.cshtml", viewModel);
        }

        public JsonResult GetCurrencies()
            => JsonData(globalSettingService.GetCurrencies());

        public JsonResult GetList(GetListGlobalSettingViewModel viewModel)
        {
            var model = !viewModel.IsValid() || !HasReadGlobalSetting(viewModel.SportType)
                ? new GlobalSettingModel()
                : globalSettingService.GetList(
                viewModel.Username,
                viewModel.SportType,
                viewModel.ExchangeId,
                viewModel.SortField,
                viewModel.SortDirection,
                viewModel.Page,
                viewModel.Size,
                viewModel.Cache);

            return JsonData(model);
        }

        public void ExportList(GetListGlobalSettingViewModel viewModel)
        {
            viewModel.Size = 0;
            viewModel.Cache = true;

            var model = !viewModel.IsValid() || !HasReadGlobalSetting(viewModel.SportType)
                ? new GlobalSettingModel()
                : globalSettingService.GetList(
                    viewModel.Username,
                    viewModel.SportType,
                    viewModel.ExchangeId,
                    viewModel.SortField,
                    viewModel.SortDirection,
                    viewModel.Page,
                    viewModel.Size,
                    viewModel.Cache);

            IExcelExporter exporter = new GlobalSettingExporter(
                viewModel.SportType,
                model.GlobalSettings,
                globalSettingService.GetProfiles(viewModel.SportType));

            exporter.ResponseStream();
        }

        [HttpPost]
        public JsonResult Add(AddGlobalSettingViewModel viewModel)
        {
            if (!viewModel.IsValid())
            {
                return BadRequest();
            }

            if (!HasHostGlobalSetting(viewModel.SportType))
            {
                return Forbidden();
            }

            var result = globalSettingService.Add(
                viewModel.Username,
                viewModel.MaxTurnovers,
                viewModel.IsGroupHeadline,
                viewModel.SportType,
                CurrentSession.UserId,
                CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult Edit(EditGlobalSettingViewModel viewModel)
        {
            if (!viewModel.IsValid())
            {
                return BadRequest();
            }

            if (!HasHostGlobalSetting(viewModel.SportType))
            {
                return Forbidden();
            }

            var param = new UpdateMaxTurnoverParam(
                viewModel.CustId,
                viewModel.MaxTurnover,
                viewModel.IsGroupHeadline,
                viewModel.SportType,
                viewModel.ProfileId);
            var result = globalSettingService.Edit(param, CurrentSession.UserId, CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult Delete(DeleteGlobalSettingViewModel viewModel)
        {
            if (!viewModel.IsValid())
            {
                return BadRequest();
            }

            if (!HasHostGlobalSetting(viewModel.SportType))
            {
                return Forbidden();
            }

            var result = globalSettingService.Delete(
                    viewModel.CustId,
                    viewModel.IsGroupHeadline,
                    viewModel.SportType,
                    CurrentSession.UserId,
                    CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        private bool HasReadGlobalSetting(short sporttype)
            => HasPermission(sporttype, Constant.Policy.Read);

        private bool HasHostGlobalSetting(short sporttype)
            => HasPermission(sporttype, Constant.Policy.Host);

        private bool HasPermission(short sporttype, Constant.Policy policy)
        {
            var permissionKey = sporttype == SportTypeConst.Soccer
                ? CMSFunctionKeys.GlobalSettingSoccer
                : CMSFunctionKeys.GlobalSettingOtherSports;

            return HasRight(permissionKey, policy);
        }
    }
}
