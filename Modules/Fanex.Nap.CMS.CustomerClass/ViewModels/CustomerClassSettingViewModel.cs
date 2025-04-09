namespace Fanex.Nap.CMS.CustomerClass.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class CustomerClassSettingViewModel : BaseViewModel
    {
        public CustomerClassSettingViewModel(
            IEnumerable<IdNameModel> sites,
            IEnumerable<short> customerClasses,
            IEnumerable<CurrencyModel> currencies,
            IEnumerable<IdNameModel> sportTypes,
            IEnumerable<LeagueGroup> leagueGroups) : base(false)
        {
            Sites = sites;
            CustomerClasses = customerClasses.Select(c => ToSelectListItem(c)).InsertAllItem();
            SportTypes = sportTypes.Select(c => ToSelectListItem(c));
            Currencies = currencies.Select(c => new IdNameModel { Id = c.ExchangeId, Name = c.Currency });
            LeagueGroups = leagueGroups;
        }

        public IEnumerable<IdNameModel> Sites { get; }

        public IEnumerable<SelectListItem> CustomerClasses { get; }

        public IEnumerable<SelectListItem> SportTypes { get; }

        public IEnumerable<IdNameModel> Currencies { get; }

        public IEnumerable<LeagueGroup> LeagueGroups { get; }
    }
}
