using Fanex.Common.Extensions;

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
            var groupedData = result
                .OrderBy(entry => entry.SportType)
                .GroupBy(entry1 => entry1.SportType)
                .Select(group1 => new
                {
                    SportType = group1.Key,
                    group1.First().SportName,
                    CurrencyGroups = group1.GroupBy(entry2 => entry2.CurrencyGroupID)
                        .Select(group2 => new
                        {
                            CurrencyGroupID = group2.Key,
                            Currency = string.Join(", ", group2.Select(e => e.Currency).Distinct()),
                            GameGroups = group2.GroupBy(entry3 => entry3.GameGroup)
                                .Select(group3 => new
                                {
                                    GameGroup = group3.Key,
                                    Entries = group3.ToList()
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            return JsonData(groupedData);
        }
    }
}