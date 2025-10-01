namespace Fanex.Nap.CMS.CustomerClass.Controllers
{
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.CustomerClass.Services;
    using Fanex.Nap.CMS.CustomerClass.ViewModels;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mime;
    using System.Web;
    using System.Web.Mvc;

    public class SpreadController : SportbookBaseController
    {
        private readonly IFilterService filterService;
        private readonly ISpreadService spreadService;
        private const byte SpreadType = 1;

        public SpreadController() : this(new FilterService(), new SpreadService(SpreadType))
        {
        }

        internal SpreadController(IFilterService filterService, ISpreadService spreadService)
        {
            this.filterService = filterService;
            this.spreadService = spreadService;
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.SpreadManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var havePermissionPortTypes = GetHavePermissionSportTypes(filterService.GetSports(), Constant.Policy.Read);
            if (!havePermissionPortTypes.Any())
            {
                return View("~/Views/Error/AccessDenied.cshtml");
            }

            var viewModel = new SpreadViewModel(
                   HasRight(CMSFunctionKeys.SpreadManagement, Constant.Policy.Host),
                   filterService.GetCustomerClasses(),
                   filterService.GetAllSites(),
                   filterService.GetAllCurrencies(),
                   havePermissionPortTypes);

            return View("~/Areas/CustomerClass/Views/Spread/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.SpreadManagement, Policy = Constant.Policy.Read)]
        public ActionResult Search(SearchSpreadRuleRequest request)
        {
            var havePermissionPortTypes =
                GetHavePermissionSportTypes(filterService.GetSports(), Constant.Policy.Read)
                    .Where(sp => request.SportTypes.Split(',')
                    .Contains(sp.Id.ToString()) || request.SportTypes == "0");
            if (!havePermissionPortTypes.Any())
            {
                return Forbidden(behavior: JsonRequestBehavior.AllowGet);
            }

            return JsonData(spreadService.GetSpreadRules(request, havePermissionPortTypes), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.SpreadManagement, Policy = Constant.Policy.Host)]
        public ActionResult Add(AddEditSpreadRuleRequest request)
        {
            if (!HavePermissionOnSportType(request.SportType, Constant.Policy.Read))
            {
                return Forbidden();
            }

            return JsonData(spreadService.AddRule(request, CurrentSession.UserId, CurrentContext.IP));
        }


        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.SpreadManagement, Policy = Constant.Policy.Host)]
        public ActionResult Update(AddEditSpreadRuleRequest request)
        {

            if (!HavePermissionOnSportType(request.SportType, Constant.Policy.Read))
            {
                return Forbidden();
            }

            return JsonData(spreadService.UpdateRule(request, CurrentSession.UserId, CurrentContext.IP), JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.SpreadManagement, Policy = Constant.Policy.Host)]
        public ActionResult Delete(int policyID, short sportType)
        {
            if (!HavePermissionOnSportType(sportType, Constant.Policy.Read))
            {
                return Forbidden();
            }

            return JsonData(spreadService.DeleteRule(policyID, CurrentSession.UserId, CurrentContext.IP), JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [NapAuth(FunctionKey = CMSFunctionKeys.SpreadManagement, Policy = Constant.Policy.Host)]
        public ActionResult Enable(int policyID, short sportType, bool enable)
        {
            if (!HavePermissionOnSportType(sportType, Constant.Policy.Read))
            {
                return Forbidden();
            }

            return JsonData(spreadService.EnableRule(policyID, enable, CurrentSession.UserId, CurrentContext.IP), JsonRequestBehavior.DenyGet);
        }

        public ActionResult DownloadTemplate()
        {
            if (!HasRight(CMSFunctionKeys.SpreadManagement, Constant.Policy.Host))
            {
                return new EmptyResult();
            }

            const string fileName = "UploadSpreadSettingTemplate.xlsx";
            var filePath = Server.MapPath($"~/App_Data/ExcelTemplates/{fileName}");

            return File(
                System.IO.File.ReadAllBytes(filePath),
                MediaTypeNames.Application.Octet,
                fileName);
        }

        [HttpPost]
        public ActionResult UploadValidate(HttpPostedFileBase file)
        {
            var userId = CurrentSession.UserId;

            if (!HasRight(CMSFunctionKeys.SpreadManagement, Constant.Policy.Host) || userId == 0)
            {
                return Forbidden();
            }

            if (file == null)
            {
                return BadRequest();
            }

            ISpreadUploadService spreadUploadService = new SpreadUploadService();
            var siteSettingExcels = spreadUploadService.ValidateExcelFile(
                file.InputStream,
                System.IO.Path.GetExtension(file.FileName),
                new SpreadSettingBodyExcelPreparer(
                    policyTitleMaxLength: 500,
                    customerClasses: filterService.GetCustomerClasses(),
                    sportTypes: filterService.GetSports(),
                    sites: filterService.GetAllSites(),
                    currencies: filterService.GetAllCurrencies(),
                    spreadRange: (0, 0.5m)
                    )
                );

            if (spreadUploadService.HasExcelErrors)
            {
                return JsonData(spreadUploadService.ExcelErrors, JsonRequestBehavior.DenyGet);
            }

            return JsonData(
                siteSettingExcels,
                JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public ActionResult UploadSubmit(IEnumerable<SpreadSettingExcelValidModel> spreadSettingExcels)
        {
            var userId = CurrentSession.UserId;
            var ip = CurrentContext.IP;
            if (!HasRight(CMSFunctionKeys.SpreadManagement, Constant.Policy.Host) || userId == 0)
            {
                return Forbidden();
            }
            if (spreadSettingExcels == null || !spreadSettingExcels.Any())
            {
                return BadRequest();
            }

            return JsonData(
                spreadService.Upload(spreadSettingExcels, userId, ip),
                JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        [NapAuth(FunctionKey = CMSFunctionKeys.SpreadManagement, Policy = Constant.Policy.Host)]
        public void ExportUploadInvalid(string request)
        {
            var data = JsonConvert.DeserializeObject<IEnumerable<SpreadSettingExcelInvalidModel>>(request);

            var exporter = new SpreadExporter(data);
            exporter.ResponseStream();
        }
    }
}
