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
            var viewModel = new FilterAreaModel(
                _service.GetProducts(true),
                _service.GetProducts(false),
                _service.GetCurrencies(true),
                _service.GetCurrencies(false),
                _service.GetSites(true),
                _service.GetSites(false)
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
    }
}