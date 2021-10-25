using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fanex.Nap.CMS.Customers.ViewModels.LimitDailyPTUpdate
{
    public class LimitDailyPTUpdateViewModel
    {
        public LimitDailyPTUpdateViewModel(IEnumerable<string> sites, IEnumerable<string> products, IEnumerable<string> currencies, bool hasHostPermission) {
            HasHostPermission = hasHostPermission;
            Sites = sites;
            Products = products;
            Currencies = currencies;
        }

        public bool HasHostPermission { get; }

        public IEnumerable<string> Sites { get; }

        public IEnumerable<string> Products { get; }

        public IEnumerable<string> Currencies { get; }

        public IEnumerable<SelectListItem> Types()
            => new[]
            {
                BuildSelectListItem("All", 0, true),
                BuildSelectListItem("Site", 1),
                BuildSelectListItem("SMA Line", 2)
            };

        public IEnumerable<SelectListItem> PopupTypes()
            => new[]
            {
                  BuildSelectListItem("SMA Line", 0, true),
                  BuildSelectListItem("Site", 1),
                  BuildSelectListItem("Currency", 2),
                  BuildSelectListItem("Site/Currency", 3),
            };

        public IEnumerable<SelectListItem> PopupSites()
            => new[]
            {
                  BuildSelectListItem("Please select", 0, true),
                  BuildSelectListItem("123", 1),
                  BuildSelectListItem("456", 2),
                  BuildSelectListItem("789", 3),
            };

        public IEnumerable<SelectListItem> PopupCurrencies()
            => new[]
            {
                  BuildSelectListItem("Please select", 0, true),
                  BuildSelectListItem("USD", 1),
                  BuildSelectListItem("VND", 2),
                  BuildSelectListItem("BTC", 3),
            };

        private static SelectListItem BuildSelectListItem(string text, int value, bool selected = false)
            => new SelectListItem { Text = text, Value = value.ToString(), Selected = selected };
    }
}
