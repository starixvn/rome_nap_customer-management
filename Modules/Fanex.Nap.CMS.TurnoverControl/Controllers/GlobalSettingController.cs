using System.Linq;
using System.Net;
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

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Index(short? t)
        {
            short sporttype = t ?? (short)SportType.Soccer;

            if (!HasReadGlobalSetting(sporttype))
            {
                return Redirect("~/Error/AccessDenied");
            }

            ViewData["SportType"] = sporttype;
            ViewData["HostPermission"] = HasHostGlobalSetting(sporttype);

            return View("~/Areas/TurnoverControl/Views/GlobalSetting/Index.cshtml");
        }

        public JsonResult GetCurrencies()
        {
            return JsonData(_globalSettingService.GetCurrencies());
        }

        public JsonResult GetList(GlobalSettingListCriteria criteria)
        {
            GlobalSettingModel model = !criteria.IsValid() || !HasReadGlobalSetting(criteria.SportType) ?
                new GlobalSettingModel(0, Enumerable.Empty<GlobalSettingItem>()) :
                _globalSettingService.GetList(criteria);

            return JsonData(model);
        }

        public void ExportList(GlobalSettingListCriteria criteria)
        {
            GlobalSettingModel model;

            if (!criteria.IsValid() || !HasReadGlobalSetting(criteria.SportType))
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
        public JsonResult Add(AddGlobalSettingCriteria criteria)
        {
            Result result;

            if (!criteria.IsValid())
            {
                result = new Result(400, Labels.BadRequest);
            }
            else if (!HasHostGlobalSetting(criteria.SportType))
            {
                result = new Result((int)HttpStatusCode.Forbidden, Labels.AccessDeniedMessage);
            }
            else
            {
                AssignUserIdAndIP(criteria);
                result = _globalSettingService.Add(criteria);
            }

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult Edit(EditGlobalSettingCriteria criteria)
        {
            Result result;

            if (!criteria.IsValid())
            {
                result = new Result(400, Labels.BadRequest);
            }
            else if (!HasHostGlobalSetting(criteria.SportType))
            {
                result = new Result((int)HttpStatusCode.Forbidden, Labels.AccessDeniedMessage);
            }
            else
            {
                AssignUserIdAndIP(criteria);
                result = _globalSettingService.Edit(criteria);
            }

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult Delete(DeleteGlobalSettingCriteria criteria)
        {
            Result result;

            if (!criteria.IsValid())
            {
                result = new Result(400, Labels.BadRequest);
            }
            else if (!HasHostGlobalSetting(criteria.SportType))
            {
                result = new Result((int)HttpStatusCode.Forbidden, Labels.AccessDeniedMessage);
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

        private bool HasReadGlobalSetting(short sporttype)
        {
            string permissionKey = sporttype == (short)SportType.Soccer ?
                CMSFunctionKeys.GlobalSettingSoccer :
                CMSFunctionKeys.GlobalSettingOtherSports;

            return HasRight(permissionKey, Constant.Policy.Read);

        }
        private bool HasHostGlobalSetting(short sporttype)
        {
            string permissionKey = sporttype == (short)SportType.Soccer ?
                CMSFunctionKeys.GlobalSettingSoccer :
                CMSFunctionKeys.GlobalSettingOtherSports;

            return HasRight(permissionKey, Constant.Policy.Host);
        }
    }
}
