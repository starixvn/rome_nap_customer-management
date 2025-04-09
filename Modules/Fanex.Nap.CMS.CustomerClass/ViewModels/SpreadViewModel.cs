namespace Fanex.Nap.CMS.CustomerClass.ViewModels
{
    using Fanex.Nap.CMS.CustomerClass.Constants;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class SpreadViewModel : BaseViewModel
    {

        public SpreadViewModel(bool hasHostPermission,
            IEnumerable<short> customerClasses,
            IEnumerable<IdNameModel> sites,
            IEnumerable<CurrencyModel> currencies,
            IEnumerable<IdNameModel> sportTypes) : base(hasHostPermission)
        {
            CustomerClasses = customerClasses;
            Sites = sites;
            Currencies = currencies.Select(c => new IdNameModel { Id = c.ExchangeId, Name = c.Currency });
            SportTypes = sportTypes.Select(c => ToSelectListItem(c));
        }

        public IEnumerable<short> CustomerClasses { get; }

        public IEnumerable<IdNameModel> Sites { get; }

        public IEnumerable<IdNameModel> Currencies { get; }

        public IEnumerable<SelectListItem> SportTypes { get; }

        public IEnumerable<IdNameModel> Platforms { get; }
            = PlatformConst.Platforms.Where(key => key.Key != 0).Select(kvp => new IdNameModel { Id = kvp.Key, Name = kvp.Value }).ToList();
    }
}
