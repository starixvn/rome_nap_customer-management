namespace Fanex.Nap.CMS.Customers.ViewModels.LimitDailyPTUpdate
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.Customers.Constants.LimitDailyPTUpdate;
    using Resources;

    public class LimitDailyPTUpdateViewModel
    {
        public LimitDailyPTUpdateViewModel(IEnumerable<SelectListItem> sites, IEnumerable<SelectListItem> products, IEnumerable<SelectListItem> currencies, bool hasHostPermission) {
            HasHostPermission = hasHostPermission;
            Sites = sites;
            Products = products;
            Currencies = currencies;
        }

        public bool HasHostPermission { get; }

        public IEnumerable<SelectListItem> Sites { get; }

        public IEnumerable<SelectListItem> Products { get; }

        public IEnumerable<SelectListItem> Currencies { get; }
        public IEnumerable<SelectListItem> LitmitNewAccountUpdatePT => new[]
        {
            BuildSelectListItem(nameof(TypeConst.All), 2, true),
            BuildSelectListItem(LimitDailyPTUpdateResource.Yes, 1),
            BuildSelectListItem(LimitDailyPTUpdateResource.No, 0)
        };

        public IEnumerable<SelectListItem> Types()
            => new[]
            {
                BuildSelectListItem(nameof(TypeConst.All), TypeConst.All, true),
                BuildSelectListItem(LimitDailyPTUpdateResource.SMALine, TypeConst.SMALine),
                BuildSelectListItem(nameof(TypeConst.Site), TypeConst.Site),
                BuildSelectListItem(nameof(TypeConst.Currency), TypeConst.Currency),
                BuildSelectListItem(LimitDailyPTUpdateResource.SiteCurrency, TypeConst.SiteCurrency)
            };

        private static SelectListItem BuildSelectListItem(string text, int value, bool selected = false)
            => new SelectListItem { Text = text, Value = value.ToString(), Selected = selected };
    }
}
