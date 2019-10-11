using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fanex.Nap.CMS.Infrastructure;
using Fanex.Nap.CMS.Infrastructure.Controllers;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
using Fanex.Nap.CMS.TurnoverControl.Criteria.ImportGlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Enums;
using Fanex.Nap.CMS.TurnoverControl.Resources;
using Fanex.Nap.CMS.TurnoverControl.Services.ImportGlobalSetting;
using Fanex.NAP.Core.Business;

namespace Fanex.Nap.CMS.TurnoverControl.Controllers
{
    public class ImportGlobalSettingController : BaseController
    {
        private readonly IExcelExporter _excelExporter;
        private readonly IImportGlobalSettingService _importService;

        public ImportGlobalSettingController()
            : this(null, null)
        {
        }

        public ImportGlobalSettingController(
            IExcelExporter excelExporter,
            IImportGlobalSettingService importService)
        {
            _excelExporter = excelExporter ?? new GlobalSettingTemplateExporter();
            _importService = importService ?? new ImportGlobalSettingService();
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Index(short? t)
        {
            short sporttype = t ?? (short)SportType.Soccer;
            ViewData["SportType"] = sporttype;

            if (!HasHostGlobalSetting(sporttype))
            {
                return Redirect("~/Error/AccessDenied");
            }

            return View("~/Areas/TurnoverControl/Views/ImportGlobalSetting/Index.cshtml");
        }

        public void DownloadTemplate()
        {
            _excelExporter.ResponseStream();
        }

        [HttpPost]
        public ActionResult Upload(short sportType, HttpPostedFileBase file)
        {
            Result result;
            SportType type = (SportType)sportType;

            if (type != SportType.Soccer && type != SportType.Others)
            {
                result = new Result(400, Infrastructure.Resources.Labels.BadRequest);
            }
            else if (!HasHostGlobalSetting(sportType))
            {
                result = new Result((int)HttpStatusCode.Forbidden, Infrastructure.Resources.Labels.AccessDeniedMessage);
            }
            else if (file == null || file.ContentLength == 0)
            {
                result = new Result(400, Labels.Message_NoReportData);
            }
            else
            {
                string extension = Path.GetExtension(file.FileName);

                result = extension.Equals(".xls", StringComparison.OrdinalIgnoreCase) ||
                    extension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase) ?
                    _importService.ReadExcelStream(type, file.InputStream, extension) :
                    new Result(400, Labels.Error_IncorrectLayout);
            }

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult Import(AddMultipleGlobalSettingCriteria criteria)
        {
            Result result;

            if (!criteria.IsValid())
            {
                result = new Result(400, Infrastructure.Resources.Labels.BadRequest);
            }
            else if (!HasHostGlobalSetting(criteria.SportType))
            {
                result = new Result((int)HttpStatusCode.Forbidden, Infrastructure.Resources.Labels.AccessDeniedMessage);
            }
            else
            {
                criteria.UserId = CurrentSession.UserId;
                criteria.IP = CurrentContext.IP;

                result = _importService.ImportGlobalSettings(criteria);
            }

            return JsonData(result, JsonRequestBehavior.DenyGet);
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
