using System.Linq;
using System.Web.Mvc;
using Fanex.Nap.CMS.Infrastructure;
using Fanex.Nap.CMS.Infrastructure.Controllers;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Resources;
using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
using Fanex.Nap.CMS.TurnoverControl.Criteria;
using Fanex.Nap.CMS.TurnoverControl.Criteria.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Enums;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Fanex.Nap.CMS.TurnoverControl.Controllers
{
    public class GlobalSettingController : BaseController
    {
        private readonly IGlobalSettingService _globalSettingService;

        public GlobalSettingController()
            : this(null)
        {
        }

        public GlobalSettingController(IGlobalSettingService globalSettingService)
        {
            _globalSettingService = globalSettingService ?? new GlobalSettingService();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Read)]
        public ActionResult Index(short? t)
        {
            ViewData["SportType"] = t ?? (short)SportType.Soccer;
            ViewData["HostPermission"] = HasRight(CMSFunctionKeys.GlobalSetting, Constant.Policy.Host);

            return View("~/Areas/TurnoverControl/Views/GlobalSetting/Index.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Read)]
        public JsonResult GetCurrencies()
        {
            return JsonData(_globalSettingService.GetCurrencies());
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Read)]
        public JsonResult GetList(GlobalSettingListCriteria criteria)
        {
            GlobalSettingModel model = !criteria.IsValid() ?
                new GlobalSettingModel(0, Enumerable.Empty<GlobalSettingItem>()) :
                _globalSettingService.GetList(criteria);

            return JsonData(model);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Read)]
        public void ExportList(GlobalSettingListCriteria criteria)
        {
            GlobalSettingModel model;

            if (!criteria.IsValid())
            {
                model = new GlobalSettingModel(0, Enumerable.Empty<GlobalSettingItem>());
            }
            else
            {
                criteria.Cache = true;
                criteria.Size = 0;
                model = _globalSettingService.GetList(criteria);
            }

            IExcelExporter exporter = new GlobalSettingExporter((SportType)criteria.SportType, model.GlobalSettings);
            exporter.ResponseStream();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Host)]
        public JsonResult Add(AddGlobalSettingCriteria criteria)
        {
            Result result;

            if (!criteria.IsValid())
            {
                result = new Result(400, Labels.BadRequest);
            }
            else
            {
                AssignUserIdAndIP(criteria);
                result = _globalSettingService.Add(criteria);
            }

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Host)]
        public JsonResult Edit(EditGlobalSettingCriteria criteria)
        {
            Result result;

            if (!criteria.IsValid())
            {
                result = new Result(400, Labels.BadRequest);
            }
            else
            {
                AssignUserIdAndIP(criteria);
                result = _globalSettingService.Edit(criteria);
            }

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Host)]
        public JsonResult Delete(DeleteGlobalSettingCriteria criteria)
        {
            Result result;

            if (!criteria.IsValid())
            {
                result = new Result(400, Labels.BadRequest);
            }
            else
            {
                AssignUserIdAndIP(criteria);
                result = _globalSettingService.Delete(criteria);
            }

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [NonAction]
        private void AssignUserIdAndIP(NonQueryCriteria criteria)
        {
            criteria.UserId = ServiceContext.UserId;
            criteria.IP = ServiceContext.Ip;
        }
    }
}
