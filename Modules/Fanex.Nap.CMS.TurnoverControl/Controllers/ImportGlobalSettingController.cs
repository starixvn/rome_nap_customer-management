using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fanex.Logging.Extensions.Web;
using Fanex.Nap.CMS.Infrastructure;
using Fanex.Nap.CMS.Infrastructure.Controllers;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.TurnoverControl.Constants;
using Fanex.Nap.CMS.TurnoverControl.Models.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Resources;
using Fanex.Nap.CMS.TurnoverControl.Services.GlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.Services.ImportGlobalSetting;
using Fanex.Nap.CMS.TurnoverControl.ViewModels.GlobalSetting;
using Fanex.NAP.Client.Helpers.Business;
using Fanex.NAP.Client.Attributes;

namespace Fanex.Nap.CMS.TurnoverControl.Controllers
{
    public class ImportGlobalSettingController : BaseController
    {
        private readonly IGlobalSettingService globalSettingService;
        private readonly IImportGlobalSettingService importService;

        public ImportGlobalSettingController()
            : this(null, null)
        {
        }

        public ImportGlobalSettingController(
            IGlobalSettingService globalSettingService,
            IImportGlobalSettingService importService)
        {
            this.globalSettingService = globalSettingService ?? new GlobalSettingService();
            this.importService = importService ?? new ImportGlobalSettingService(this.globalSettingService);
        }

        [OutputCache(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Index(short? t)
        {
            short sportType = t ?? SportTypeConst.Soccer;
            var hasHostPermission = HasHostGlobalSetting(sportType);

            if (!hasHostPermission)
            {
                return Redirect("~/Error/AccessDenied");
            }

            return View(
                "~/Areas/TurnoverControl/Views/ImportGlobalSetting/Index.cshtml",
                new GlobalSettingViewModel(sportType, hasHostPermission, null, globalSettingService.GetProfiles(sportType)));
        }

        public void DownloadTemplate(short sportType)
        {
            if (SportTypeConst.IsValidSportType(sportType))
            {
                var profiles = globalSettingService.GetProfiles(sportType);
                var excelExporter = new GlobalSettingTemplateExporter(profiles);

                excelExporter.ResponseStream();
            }
        }

        [HttpPost]
        public ActionResult Upload(short sportType, HttpPostedFileBase file)
        {
            if (!SportTypeConst.IsValidSportType(sportType))
            {
                return BadRequest();
            }

            if (!HasHostGlobalSetting(sportType))
            {
                return Forbidden();
            }

            if (file == null || file.ContentLength == 0)
            {
                return BadRequest(Labels.Message_NoReportData);
            }

            Result result;

            try
            {
                result = importService.ReadExcelStream(
                    sportType,
                    file.InputStream,
                    Path.GetExtension(file.FileName));
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
                result = new Result((int)HttpStatusCode.BadRequest, Labels.Error_IncorrectLayout);
            }

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult Import(AddGlobalSettingViewModel[] viewModel)
        {
            var firstItem = viewModel?.FirstOrDefault();

            if (firstItem == null || !firstItem.IsValid())
            {
                return BadRequest();
            }

            if (!HasHostGlobalSetting(firstItem.SportType))
            {
                return Forbidden();
            }

            var result = importService.ImportGlobalSettings(
                firstItem.SportType,
                viewModel.Select(vm => new GlobalSettingCustomer(vm.Username, vm.IsGroupHeadline, vm.MaxTurnovers)),
                CurrentSession.UserId,
                CurrentContext.IP);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        private bool HasHostGlobalSetting(short sporttype)
        {
            var permissionKey = sporttype == SportTypeConst.Soccer
                ? CMSFunctionKeys.GlobalSettingSoccer
                : CMSFunctionKeys.GlobalSettingOtherSports;

            return HasRight(permissionKey, Constant.Policy.Host);
        }
    }
}
