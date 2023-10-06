using System.Linq;

namespace Fanex.Nap.CMS.ManageProductSetting.Controllers
{
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using System.Web.Mvc;
    using ViewModels.CompanyBetLimitSetting;
    using Services.CompanyBetLimitSetting;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;

    public class CompanyBetLimitSettingController : BaseController
    {
        // GET: CompanyBetLimitSetting
        private readonly ICompanyBetLimitSettingService _service;

        public CompanyBetLimitSettingController() : this(null)
        {
        }

        internal CompanyBetLimitSettingController(ICompanyBetLimitSettingService service)
        {
            this._service = service ?? new CompanyBetLimitSettingService();
        }


        public ActionResult Index()
        {
            var viewModel = new FilterAreaModel(
                _service.GetProducts(true),
                _service.GetProducts(false),
                _service.GetCurrencies(true),
                _service.GetCurrencies(false)
            );
            return View("~/Areas/ManageProductSetting/Views/CompanyBetLimitSetting/Index.cshtml", viewModel);
        }

        public ActionResult Search(GetCompanyBetLimitSettingViewModels searchParams)
        {
            var result = _service.GetBetLimitSetting(
                searchParams.IsCredit,
                searchParams.ProductIds,
                searchParams.Currencies,
                searchParams.PageIndex,
                searchParams.PageSize
            );

            var groupedData = _service.GetBetLimitSettingGroups(result.Entries);

            return JsonData(new
            {
                Data = groupedData,
                result.TotalRows
            });
        }

        public void Export(GetCompanyBetLimitSettingViewModels searchParams)
        {
            var result = _service.GetBetLimitSetting(
                searchParams.IsCredit,
                searchParams.ProductIds,
                searchParams.Currencies,
                searchParams.PageIndex,
                searchParams.PageSize
            );

            var groupedData = _service.GetBetLimitSettingGroups(result.Entries);
            IExcelExporter exporter = new CompanyBetLimitSettingExporter(groupedData);

            exporter.ResponseStream();
        }
    }
}