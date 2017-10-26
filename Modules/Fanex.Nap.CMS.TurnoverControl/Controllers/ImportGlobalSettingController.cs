using System;
using System.IO;
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
using Fanex.NAP.Core.Web.Attribute;

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

        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Host)]
        public ActionResult Index(short? t)
        {
            ViewData["SportType"] = t ?? (short)SportType.Soccer;

            return View("~/Areas/TurnoverControl/Views/ImportGlobalSetting/Index.cshtml");
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Host)]
        public void DownloadTemplate()
        {
            _excelExporter.ResponseStream();
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Host)]
        public ActionResult Upload(short sportType, HttpPostedFileBase file)
        {
            Result result;
            SportType type = (SportType)sportType;

            if (type != SportType.Soccer && type != SportType.Others)
            {
                result = new Result(400, Infrastructure.Resources.Labels.BadRequest);
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
        [NapAuth(FunctionKey = CMSFunctionKeys.GlobalSetting, Policy = Constant.Policy.Host)]
        public ActionResult Import(AddMultipleGlobalSettingCriteria criteria)
        {
            Result result;

            if (!criteria.IsValid())
            {
                result = new Result(400, Infrastructure.Resources.Labels.BadRequest);
            }
            else
            {
                criteria.UserId = ServiceContext.UserId;
                criteria.IP = ServiceContext.Ip;

                result = _importService.ImportGlobalSettings(criteria);
            }

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }
    }
}
