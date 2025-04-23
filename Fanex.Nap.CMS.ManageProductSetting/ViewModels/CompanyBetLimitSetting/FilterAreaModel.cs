namespace Fanex.Nap.CMS.ManageProductSetting.ViewModels.CompanyBetLimitSetting
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Linq;
    using Fanex.Nap.CMS.ManageProductSetting.Resources;

    public class FilterAreaModel
    {
        public IEnumerable<SelectListItem> CreditCurrencies { get; }

        public IEnumerable<SelectListItem> DepositCurrencies { get; }

        public IEnumerable<SelectListItem> DepositProducts { get; }

        public IEnumerable<SelectListItem> CreditProducts { get; }

        public IEnumerable<SelectListItem> CreditSites { get; }

        public IEnumerable<SelectListItem> DepositSites { get; }

        public IEnumerable<SelectListItem> GroupTypes { get; }

        public FilterAreaModel(
            IEnumerable<IdNameModel> creditProducts,
            IEnumerable<IdNameModel> depositProducts,
            IEnumerable<IdNameModel> creditCurrencies,
            IEnumerable<IdNameModel> depositCurrencies,
            IEnumerable<IdNameModel> creditSites,
            IEnumerable<IdNameModel> depositSites)
        {
            CreditCurrencies = creditCurrencies.ToList()
                .ConvertAll(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() });
            DepositCurrencies = depositCurrencies.ToList()
                .ConvertAll(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() });
            CreditProducts = creditProducts.ToList()
                .ConvertAll(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() });
            DepositProducts = depositProducts.ToList()
                .ConvertAll(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() });
            CreditSites = creditSites.ToList()
                .ConvertAll(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() });
            DepositSites = depositSites.ToList()
                .ConvertAll(e => new SelectListItem { Text = e.Name, Value = e.Id.ToString() });
            GroupTypes = new List<SelectListItem>()
            {
                new SelectListItem { Text = Labels.Credit, Value = "Credit" },
                new SelectListItem { Text = Labels.Deposit, Value = "Deposit" }
            };
        }
    }
}