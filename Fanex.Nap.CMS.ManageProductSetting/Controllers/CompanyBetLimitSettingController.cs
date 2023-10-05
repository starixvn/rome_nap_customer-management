using Fanex.Nap.CMS.ManageProductSetting.Models.CompanyBetLimitSetting;

namespace Fanex.Nap.CMS.ManageProductSetting.Controllers
{
    using Fanex.Nap.CMS.Infrastructure.Controllers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.ManageProductSetting.ViewModels.CompanyBetLimitSetting;
    using Fanex.Nap.CMS.ManageProductSetting.Services.CompanyBetLimitSetting;
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
                searchParams.Currencies
            );

            var groupedData = _service.GetBetLimitSettingGroups(result);
         
            return JsonData(groupedData);
        }

        public void Export(GetCompanyBetLimitSettingViewModels searchParams)
        {
            var result = _service.GetBetLimitSetting(
                searchParams.IsCredit,
                searchParams.ProductIds,
                searchParams.Currencies
            );

            var groupedData = _service.GetBetLimitSettingGroups(result);
            IExcelExporter exporter = new CompanyBetLimitSettingExporter(groupedData);

            exporter.ResponseStream();
        }
    }
}