namespace Fanex.Nap.CMS.CustomerClass.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mime;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Services;
    using Fanex.Nap.CMS.CustomerClass.ViewModels;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    public class SiteController : BaseController
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
            var viewModel = new SiteViewModel(
                HasRight(CMSFunctionKeys.SiteManagement, Constant.Policy.Host),
                filterService.GetAllSites(),
                policyService.GetPolicyTitles(),
                filterService.GetAllCurrencies());
            return View("~/Areas/CustomerClass/Views/Site/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.SiteManagement, Policy = Constant.Policy.Read)]
        public ActionResult Search(GetSiteSettingRequest request)
            => JsonData(siteService.GetPagedSiteSetting(request));


        [NapAuth(FunctionKey = CMSFunctionKeys.SiteManagement, Policy = Constant.Policy.Host)]
        public ActionResult GetPolicyTitle(short sportType)
            => JsonData(policyService.GetPolicyTitles(sportType));

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.SiteManagement, Policy = Constant.Policy.Host)]
        public ActionResult Assign(AssignSiteSettingRequest request, bool isOverwrite)
        {
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

            ISiteUploadService siteUploadService = new SiteUploadService();
            var siteSettingExcels = siteUploadService.ValidateExcelFile(
                file.InputStream,
                System.IO.Path.GetExtension(file.FileName),
                filterService.GetAllSites(),
                filterService.GetAllCurrencies(),
                policyService.GetPolicyTitles());

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
