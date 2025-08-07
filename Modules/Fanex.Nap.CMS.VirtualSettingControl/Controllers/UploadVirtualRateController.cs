namespace Fanex.Nap.CMS.VirtualSettingControl.Controllers
{
    using System.IO;
    using System.Net;
    using System.Net.Mime;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.VirtualSettingControl.Constants;
    using Fanex.NAP.CMS.Core.Resources;
    using Fanex.NAP.Client.Helpers.Business;
    using Fanex.NAP.Client.Attributes;
    using Infrastructure;
    using Infrastructure.Controllers;
    using Services.UploadVirtualRate;

    public class UploadVirtualRateController : BaseController
    {
        [NapAuth(FunctionKey = CMSFunctionKeys.ESports_UploadVirtualRate, Policy = Constant.Policy.Read)]
        public ActionResult Index()
            => IndexView(SportTypeConstants.ESports);

        [NapAuth(FunctionKey = CMSFunctionKeys.Basketball_UploadVirtualRate, Policy = Constant.Policy.Read)]
        public ActionResult Basketball()
            => IndexView(SportTypeConstants.Basketball);

        public ActionResult DownloadTemplate(int sportType)
        {
            if (HasRead(sportType))
            {
                var path = Server.MapPath("~/App_Data/ExcelTemplates");
                var fileName = $"{CacheRef.GetSportName(sportType)}-UploadVirtualRateTemplate.xlsx";
                var fileBytes = System.IO.File.ReadAllBytes($"{path}/{fileName}");

                return File(fileBytes, MediaTypeNames.Application.Octet, fileName);
            }

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult VerifyByWeb(HttpPostedFileBase file, int sportType)
        {
            if (HasHost(sportType))
            {
                var service = GetUploadService(sportType);
                var errors = service.VerifyByWeb(file?.InputStream, Path.GetExtension(file?.FileName));

                return JsonData(errors, JsonRequestBehavior.DenyGet);
            }

            return JsonData(new Result((int)HttpStatusCode.Forbidden, Messages.AccessDenied), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult VerifyByDB(HttpPostedFileBase file, int sportType, int startRow)
        {
            if (HasHost(sportType))
            {
                var service = GetUploadService(sportType);
                var errors = service.VerifyByDB(
                    file?.InputStream,
                    Path.GetExtension(file?.FileName),
                    CurrentSession.UserId,
                    startRow);

                return JsonData(errors, JsonRequestBehavior.DenyGet);
            }

            return JsonData(new Result((int)HttpStatusCode.Forbidden, Messages.AccessDenied), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult Update(int sportType)
        {
            var result = HasHost(sportType)
                ? GetUploadService(sportType).Update(CurrentSession.UserId, CurrentContext.IP)
                : new Result((int)HttpStatusCode.Forbidden, Messages.AccessDenied);

            return JsonData(result, JsonRequestBehavior.DenyGet);
        }

        private IUploadVirtualRateService GetUploadService(int sportType)
            => new UploadVirtualRateService(sportType);

        private ActionResult IndexView(int sportType)
        {
            ViewBag.SportTypeName = CacheRef.GetSportName(sportType);
            ViewBag.LimitAccount = LimitAccountConstants.GetLimitAccount(sportType);
            ViewBag.UpdateBatchSize = UploadVirtualRateService.UpdateBatchSize;

            return View("~/Areas/VirtualSettingControl/Views/UploadVirtualRate/Index.cshtml", sportType);
        }

        private bool HasRead(int sportType)
            => HasPermission(sportType, Constant.Policy.Read);

        private bool HasHost(int sportType)
            => HasPermission(sportType, Constant.Policy.Host);

        private bool HasPermission(int sportType, Constant.Policy policy)
            => HasRight(MapSportTypeToFunctionKey(sportType), policy);

        private static string MapSportTypeToFunctionKey(int sportType)
        {
            if (sportType == SportTypeConstants.ESports)
            {
                return CMSFunctionKeys.ESports_UploadVirtualRate;
            }

            if (sportType == SportTypeConstants.Basketball)
            {
                return CMSFunctionKeys.Basketball_UploadVirtualRate;
            }

            return null;
        }
    }
}
