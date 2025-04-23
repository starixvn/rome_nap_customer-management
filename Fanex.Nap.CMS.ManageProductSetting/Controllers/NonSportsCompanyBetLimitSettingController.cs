namespace Fanex.Nap.CMS.ManageProductSetting.Controllers
{
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using System.Web.Mvc;
    using ViewModels.CompanyBetLimitSetting;
    using Services.CompanyBetLimitSetting;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using Fanex.Nap.CMS.Infrastructure;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using System.Collections.Generic;
    using Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting;
    using System.Net.Mime;
    using System.Web;
    using System.IO;

    public class NonSportsCompanyBetLimitSettingController : BaseController
    {
        private readonly ICompanyBetLimitSettingService _service;

        public NonSportsCompanyBetLimitSettingController() : this(null)
        {
        }

        internal NonSportsCompanyBetLimitSettingController(ICompanyBetLimitSettingService service)
        {
            this._service = service ?? new CompanyBetLimitSettingService();
        }
        
        [NapAuth(FunctionKey = CMSFunctionKeys.Non_sportsCompanyBetLimitSetting, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            ViewBag.IsHost = HasRight(CMSFunctionKeys.Non_sportsCompanyBetLimitSetting, Constant.Policy.Host);
            var viewModel = new FilterAreaModel(
                _service.GetProducts(true),
                _service.GetProducts(false),
                _service.GetCurrencies(true),
                _service.GetCurrencies(false),
                _service.GetSites(true, false),
                _service.GetSites(false, false)
            );
            return View("~/Areas/ManageProductSetting/Views/CompanyBetLimitSetting/Index.cshtml", viewModel);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Non_sportsCompanyBetLimitSetting, Policy = Constant.Policy.Read)]
        public ActionResult Search(GetCompanyBetLimitSettingViewModels searchParams)
        {
            var result = _service.GetBetLimitSetting(searchParams);

            var groupedData = _service.GetBetLimitSettingGroups(result);

            return JsonData(groupedData);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Non_sportsCompanyBetLimitSetting, Policy = Constant.Policy.Read)]
        public void Export(GetCompanyBetLimitSettingViewModels searchParams)
        {
            var result = _service.GetBetLimitSetting(searchParams);
            var groupedData = _service.GetBetLimitSettingGroups(result);
            
            IExcelExporter exporter = new CompanyBetLimitSettingExporter(groupedData,searchParams.IsCredit);
            exporter.ResponseStream();
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Non_sportsCompanyBetLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult Update(IEnumerable<UpdateBetLimitSettingModel> settings)
        {
            var result = _service.UpdateBetLimitSetting(settings, CurrentSession.UserId, CurrentContext.IP);

            return JsonData(result);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Non_sportsCompanyBetLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult Delete(bool isCredit, IEnumerable<CompanyBetLimitSetting> settings)
        {
            var result = _service.DeleteBetLimitSetting(settings, isCredit, CurrentSession.UserId, CurrentContext.IP);

            return JsonData(result);
        }


        [NapAuth(FunctionKey = CMSFunctionKeys.Non_sportsCompanyBetLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult DownloadTemplate()
        {
            var path = Server.MapPath("~/App_Data/ExcelTemplates/CompanyBetLimit");
            var fileName = $"CompanyBetLimit.xls";
            var fileBytes = System.IO.File.ReadAllBytes($"{path}/{fileName}");

            return File(fileBytes, MediaTypeNames.Application.Octet, fileName);
        }


        [NapAuth(FunctionKey = CMSFunctionKeys.Non_sportsCompanyBetLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult ValidateExcel(HttpPostedFileBase file)
        {
            var result = _service.VerifyByWeb(file?.InputStream, Path.GetExtension(file?.FileName), CurrentSession.UserId, CurrentContext.IP);

            return JsonData(result);
        }

        [NapAuth(FunctionKey = CMSFunctionKeys.Non_sportsCompanyBetLimitSetting, Policy = Constant.Policy.Host)]
        public ActionResult ImportExcel(IEnumerable<CompanyBetLimitSettingExcel> request)
        {
            var result = _service.ImportExcel(request, CurrentSession.UserId, CurrentContext.IP);

            return JsonData(result);
        }
    }
}