namespace Fanex.Nap.CMS.CustomerClass.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mime;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Services;
    using Fanex.Nap.CMS.CustomerClass.ViewModels;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class SiteController : SportbookBaseController
    {
        private readonly IFilterService filterService;
        private readonly IPolicyService policyService;
        private readonly ISiteService siteService;

        public SiteController()
            : this(new FilterService(), new PolicyService(), new SiteService())
        {
        }

        internal SiteController(
            IFilterService filterService,
            IPolicyService policyService,
            ISiteService siteService)
        {
            this.filterService = filterService;
            this.policyService = policyService;
            this.siteService = siteService;
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.SiteManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var havePermissionPortTypes = GetHavePermissionSportTypes(filterService.GetSports(), Constant.Policy.Read);
            if (!havePermissionPortTypes.Any())
            {
                return View("~/Views/Error/AccessDenied.cshtml");
            }

            var viewModel = new SiteViewModel(
                HasRight(CMSFunctionKeys.SiteManagement, Constant.Policy.Host),
                filterService.GetAllSites(),
                policyService.GetPolicyTitles(string.Join(",", havePermissionPortTypes.Select(sp => sp.Id))),
                filterService.GetAllCurrencies(),
                havePermissionPortTypes);
            return View("~/Areas/CustomerClass/Views/Site/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.SiteManagement, Policy = Constant.Policy.Read)]
        public ActionResult Search(GetSiteSettingRequest request)
        {
            var havePermissionPortTypes = GetHavePermissionSportTypes(filterService.GetSports(), Constant.Policy.Read).Where(sp => sp.Id == request.SportType || request.SportType == 0);
            if (!havePermissionPortTypes.Any())
            {
                return Forbidden(behavior: JsonRequestBehavior.AllowGet);
            }

            return JsonData(siteService.GetPagedSiteSetting(request, string.Join(",", havePermissionPortTypes.Select(sp => sp.Id))), JsonRequestBehavior.AllowGet);
        }


        [NapAuth(FunctionKey = CMSFunctionKeys.SiteManagement, Policy = Constant.Policy.Host)]
        public ActionResult GetPolicyTitle(short sportType)
        {
            if (!HavePermissionOnSportType(sportType, Constant.Policy.Read))
            {
                return Forbidden(behavior: JsonRequestBehavior.AllowGet);
            }

            return JsonData(policyService.GetPolicyTitles(sportType.ToString()), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.SiteManagement, Policy = Constant.Policy.Host)]
        public ActionResult Assign(AssignSiteSettingRequest request, bool isOverwrite)
        {
            if (!HavePermissionOnSportType(request.SportType, Constant.Policy.Read))
            {
                return Forbidden();
            }

            request.AssignUserIdAndIP(CurrentSession, CurrentContext);

            if (!request.IsValid())
            {
                return BadRequest();
            }

            if (request.IsCheck)
            {
                var duplicatedSiteSettings = siteService.GetDuplicatedSiteSettings(request).ToList();

                if (duplicatedSiteSettings.Count > 0)
                {
                    return JsonData(duplicatedSiteSettings, JsonRequestBehavior.DenyGet);
                }
            }

            return JsonData(siteService.AssignSiteSetting(request, isOverwrite), JsonRequestBehavior.DenyGet);
        }

        public ActionResult DownloadTemplate()
        {
            if (!HasRight(CMSFunctionKeys.SiteManagement, Constant.Policy.Host))
            {
                return new EmptyResult();
            }

            const string fileName = "UploadSiteSettingTemplate.xlsx";
            var filePath = Server.MapPath($"~/App_Data/ExcelTemplates/{fileName}");

            return File(
                System.IO.File.ReadAllBytes(filePath),
                MediaTypeNames.Application.Octet,
                fileName);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file, bool isOverwrite)
        {
            var userId = CurrentSession.UserId;
            var ip = CurrentContext.IP;

            if (!HasRight(CMSFunctionKeys.SiteManagement, Constant.Policy.Host) || userId == 0)
            {
                return Forbidden();
            }

            if (file == null)
            {
                return BadRequest();
            }

            var havePermissionPortTypes = GetHavePermissionSportTypes(filterService.GetSports(), Constant.Policy.Read);
            if (!havePermissionPortTypes.Any())
            {
                return Forbidden();
            }

            var policyTitles = policyService.GetPolicyTitles(string.Join(",", havePermissionPortTypes.Select(sp => sp.Id)));

            ISiteUploadService siteUploadService = new SiteUploadService();
            var siteSettingExcels = siteUploadService.ValidateExcelFile(
                file.InputStream,
                System.IO.Path.GetExtension(file.FileName),
                filterService.GetAllSites(),
                filterService.GetAllCurrencies(),
                policyTitles);

            if (siteUploadService.HasExcelErrors)
            {
                return JsonData(siteUploadService.ExcelErrors, JsonRequestBehavior.DenyGet);
            }

            return JsonData(
                siteService.Upload(siteSettingExcels, isOverwrite, userId, ip),
                JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.SiteManagement, Policy = Constant.Policy.Host)]
        public ActionResult Delete(DeleteSiteSettingRequest request)
        {
            var userId = CurrentSession.UserId;
            var ip = CurrentContext.IP;

            if (request.SiteSettings == null || !request.SiteSettings.Any() || userId == 0)
            {
                return BadRequest();
            }

            return JsonData(
                siteService.Delete(request, userId, ip),
                JsonRequestBehavior.DenyGet);
        }
    }
}
